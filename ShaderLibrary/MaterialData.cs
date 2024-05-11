namespace UnityEngine.Rendering.TooYoung
{
    public enum MediumType
    {
        Air = 0,
        
    }

    
    [GenerateHLSL(generateCBuffer = true)]
    public struct MaterialProperties
    {
        public Color albedoColor;

        public float roughness;

        public float metallic;
        
        /// <summary>
        /// https://en.wikipedia.org/wiki/Mean_free_path
        /// In physics, mean free path is the average distance over which a moving particle
        /// (such as an atom, a molecule, or a photon) travels before substantially
        /// changing its direction or energy (or, in a specific context, other properties),
        /// typically as a result of one or more successive collisions with other particles.
        /// </summary>
        /// unit is nm，10^-9 m
        public float meanFreePath;

        public float density;

        public float refractiveIndex;
        
    }
}