# AlexaSkillsKit.NET
.NET library to write Alexa skills that's interface-compatible with [Amazon's AlexaSkillsKit for Java](https://github.com/amzn/alexa-skills-kit-java) and matches that functionality:
* handles the (de)serialization of Alexa requests & responses into easy-to-use object models
* verifies authenticity of the request by validating its signature and timestamp
* code-reviewed and vetted by Amazon (Alexa skills written using this library passed certification)

This library will be available as a NuGet package: https://www.nuget.org/packages/Ra.AlexaSkillsKit/

To install Ra.AlexaSkillsKit, run the following command in the Package Manager Console
``
Install-Package Ra.AlexaSkillsKit
``


# How To Use

### 1. Set up your development environment

Read [Getting started with Alexa App development for Amazon Echo using .NET on Windows](https://freebusy.io/blog/getting-started-with-alexa-app-development-for-amazon-echo-using-dot-net)

### 2. Implement your skill as a "Speechlet"

If your Alexa skill does any kind of I/O and assuming you're building on top of .NET Framework 4.5/4.62 or .Net Standard >= 1.4 it's recommended that you derive your app from the abstract SpeechletApp and implement these methods as defined by ISpeechlet
   

```csharp
public interface ISpeechlet
{
        SpeechletResponse OnIntent(IntentRequest intentRequest, Session session, Context context);
        SpeechletResponse OnAudioIntent(AudioPlayerRequest audioRequest, Session session, Context context);
        SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session, Context context);

        void OnSessionStarted(SpeechletRequestEnvelope requestEnvelope);
        void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session, Context context);


        void OnRequestIncome(string msg);
        void OnResonseOutgoing(string msg);
        void OnParsingError(Exception exception);
}
```
### 3. Default Responses
  
Create a default response with one line
```csharp

Say(..)
SayWithCard(..)
SayWithLinkAccountCard(..)

AudioPlayer_Play(..)
AudioPlayer_Stop()
AudioPlayer_ClearQueue(..)
DialogDelegate()
DialogElicitSlot(..)
DialogConfirmSlot(..)

Error_GenericError() // Sorry, the application encountered an error
Error_NoIntentFound() // Sorry, the application didn't know what to do with that intent
Error_NoLaunchFunction() // Try telling the application what to do instead of opening it


```
### 4. SSML Builder
Easy to use Builder for SSML responses
https://developer.amazon.com/public/solutions/alexa/alexa-skills-kit/docs/speech-synthesis-markup-language-ssml-reference

```csharp
var ssml = new SsmlBuilder();
ssml.Append("No");
ssml.Break(1500);
ssml.Wisper("The correct answer is 42");

// Return a SSML Answer
return Say(ssml);         
```

### 5. Wire-up "Speechlet" to HTTP hosting environment

The Sample app is using ASP.NET 4.62 WebApi 2 so wiring-up requests & responses from the HTTP hosting environment (i.e. ASP.NET) to the "Speechlet" is just a matter of writing a 2-line ApiController like this 
https://github.com/realares/Ra.AlexaSkillsKit.NET/blob/master/Ra.AlexaSkillsKit.WebSample/Controllers/AlexaController.cs

```csharp
[Route("api/alexaSample")]
[HttpPost]
public HttpResponseMessage AlexaSampleRequest()
{
	var speechlet = new AlexaSample42App();
	return speechlet.GetResponse(Request);
}

```

Alternatively you can host your app and the Ra.AlexaSkillsKit.NET library in any other web service framework like ServiceStack.

