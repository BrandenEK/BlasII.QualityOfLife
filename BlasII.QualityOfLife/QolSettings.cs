using Newtonsoft.Json;

namespace BlasII.QualityOfLife
{
    internal class QolSettings
    {
        public bool allowMirabrasGlitches;

        [JsonConstructor]
        public QolSettings(bool allowMirabrasGlitches)
        {
            this.allowMirabrasGlitches = allowMirabrasGlitches;
        }

        public QolSettings()
        {
            allowMirabrasGlitches = true;
        }
    }
}
