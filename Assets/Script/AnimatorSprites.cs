using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSprites : MonoBehaviour
{

    [SerializeField] private Material[] SadMaterials;  // Materiais padrão (de frente)
    [SerializeField] private Material[] HappyMaterials;   // Materiais para as costas

    public SkinnedMeshRenderer _renderer;


    public void SadFace()
    {

        _renderer.materials = SadMaterials;

    }


    public void HappyFace()
    {

        _renderer.materials = HappyMaterials;

    }

}
