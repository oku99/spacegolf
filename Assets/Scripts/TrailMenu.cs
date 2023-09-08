using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

// Makes changes to the Particle Trail
public class TrailMenu : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private UnityEngine.UI.Slider densitySlider;
    [SerializeField] private UnityEngine.UI.Slider thicknessSlider;
    [SerializeField] private UnityEngine.UI.Slider redSlider;
    [SerializeField] private UnityEngine.UI.Slider greenSlider;
    [SerializeField] private UnityEngine.UI.Slider blueSlider;
    [SerializeField] private UnityEngine.UI.Slider randomSlider;

    [SerializeField] private TextMeshProUGUI enableButtonText;
    private bool _particlesEnabled = true;

    public void SetRed()
    {
        var main = ps.main;
        main.startColor = Color.red;
    }
    
    public void SetGreen()
    {
        var main = ps.main;
        main.startColor = Color.green;
    }
    
    
    public void SetBlue()
    {
        var main = ps.main;
        main.startColor = Color.blue;
    }

    public void SetCustom()
    {
        var main = ps.main;
        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value, main.startColor.color.a);
        main.startColor = newColor;
    }

    public void SetDensity()
    {
        var emission = ps.emission;
        emission.rateOverDistance = densitySlider.value; 
    }

    public void SetThickness()
    {
        var shape = ps.shape;
        shape.radius = thicknessSlider.value;
    }

    public void SetRandomness()
    {
        var noise = ps.noise;
        noise.strengthMultiplier = randomSlider.value;
    }

    public void ToggleEnable()
    {
        if (_particlesEnabled)
        {
            _particlesEnabled = false;
            enableButtonText.text = "ENABLE";
            ps.Pause();
        }
        else
        {
            _particlesEnabled = true;
            enableButtonText.text = "DISABLE";
            ps.Play();
        }
    }
}


/*
 * CODE ADAPTED FROM
 *      - "Noise module" from Unity Documentation
 *          - https://docs.unity3d.com/Manual/PartSysNoiseModule.html
 *      - "Slider.value" from Unity Documentation
 *          - https://docs.unity3d.com/2018.2/Documentation/ScriptReference/UI.Slider.html
*/