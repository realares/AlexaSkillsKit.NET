namespace Ra.AlexaSkillsKit.Directives.RenderTemplates
{
    public enum BodyTemplateTypeEnum
    {
        /// <summary>
        /// <para>Full-width text</para>
        /// <para>Display Size (pixels): Maximum 340 x 340,
        /// Title (optional),
        /// Skill icon (provided in developer portal),
        /// Rich or Plain text,
        /// Background image (optional)</para>
        /// <para><see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_BodyTemplate_01_50._TTH_.png"/></para>
        /// </summary>
        BodyTemplate1,

        /// <summary>
        /// <para>Image right</para>
        /// <para>Title (optional),
        /// Skill icon (provided by developer portal),
        /// Image (optional - can be a rectangle or square),
        /// Rich or Plain text,
        /// Background image (optional),
        /// One image required,
        /// Scrollable,
        /// Expandable text area</para>
        /// Display Size (pixels): Maximum 340 x 340 
        /// <para><see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_BodyTemplate_02_50._TTH_.png"/></para>
        /// </summary>
        BodyTemplate2,

        /// <summary>
        /// <para>Title (optional)</para>
        /// <para>Skill icon,
        /// Image (optional - can be a rectangle or square),
        /// Rich or Plain text,
        /// Background image (optional)
        /// </para>
        /// Display Size (pixels): Maximum 340 x 340
        /// <para><see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_BodyTemplate_03_50._TTH_.png"/></para>
        /// </summary>
        BodyTemplate3,

        /// <summary>
        /// <para>Full-screen image with text overlay</para>
        /// <para>Display Size (pixels): Maximum 340 x 340,
        /// No title,
        /// Skill icon,
        /// One full-screen image(1024 x 600 for background),
        /// Rich or Plain text,
        /// Can be used as a welcome screen to offer guidance.
        /// Image does not scroll, but the text does.</para>
        /// <para><see cref="https://images-na.ssl-images-amazon.com/images/G/01/mobile-apps/dex/ask-customskills/Show_BodyTemplate_06_50._TTH_.png"/></para>
        /// </summary>
        BodyTemplate6
    }

}
