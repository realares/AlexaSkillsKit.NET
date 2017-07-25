using NLog;
using Ra.AlexaSkillsKit.WebSample.AlexaApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ra.AlexaSkillsKit.WebSample.Controllers
{
    public class AlexaController : ApiController
    {
        public static Logger log = LogManager.GetCurrentClassLogger();

        [Route("api/alexaSample")]
        [HttpPost]
        public HttpResponseMessage AlexaSampleRequest()
        {
            try
            {
                var speechlet = new AlexaSample42App();
                return speechlet.GetResponse(Request);
            }
            catch (Exception e)
            {
                log.Error(e, "GeneralExeption");
            }
            return null;
        }

    }
}
