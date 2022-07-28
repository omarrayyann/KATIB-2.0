using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

using TMPro;

using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;



/// <summary>
/// UnityEvent which can pass a Texture as an argument.
/// </summary>
[Serializable]
public class UnityEventFont : UnityEvent<TMP_FontAsset> { }

/// <summary>
/// Component that can be used to Localize a Texture asset.
/// Provides an update event that can be used to automatically update the texture whenever the Locale changes.
/// </summary>
[AddComponentMenu("Localization/Asset/Localize TMPro Font Event")]
public class LocalizeTMProFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMProFont, UnityEventFont>
{
    protected override void UpdateAsset(TMP_FontAsset localizedAsset)
    {
        TMP_Text text = this.gameObject.GetComponent<TMP_Text>();
        text.font = localizedAsset;
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        if ((text.alignment.Equals(TextAlignmentOptions.MidlineLeft) || text.alignment.Equals(TextAlignmentOptions.TopLeft) || text.alignment.Equals(TextAlignmentOptions.BottomLeft)) && currentLocale.ToString().Equals("Arabic (ar)"))
        {
            Debug.Log("This is Arabic");
            text.alignment = TextAlignmentOptions.Right;
        }
        else if (text.alignment == TextAlignmentOptions.Right)
        {
            text.alignment = TextAlignmentOptions.Left;
        }
    }
}

