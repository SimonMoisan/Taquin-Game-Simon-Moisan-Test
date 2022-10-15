using System.Collections.Generic;
using UnityEngine;

public class ImageBank : MonoBehaviour
{
    [SerializeField] public List<ImagePreset> imagePresets;

    [System.Serializable]
    public class ImagePreset
    {
        public ApplicationPlateform plateformTarget;
        public List<ImagePiece> imagePieces;
        public Sprite fullImage;
    }
}
