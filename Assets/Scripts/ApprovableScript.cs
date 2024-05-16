using productRelated;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;

public class ApprovableScript : MonoBehaviour
{
    private ProductData _self;

    //[SerializeField] private TMP_Text masterName;
    [SerializeField] private TMP_Text productName;
    [SerializeField] private TMP_Text productDescription;

    public ProductData Approvable => _self; 
    
    public void SetValues(ProductData approvableData)
    {
        try
        {
            _self = approvableData;
            //masterName.SetText(approvableData.vendorUserLogin);
            productName.SetText(approvableData.name);
            productDescription.text = approvableData.description;

            Vector4 fieldMargins = productDescription.margin;
            productDescription.ForceMeshUpdate();
            fieldMargins.w = -productDescription.GetRenderedValues(true).y;
            productDescription.margin = fieldMargins;
        }
        catch (Exception ex)
        {
            Debug.LogError("Approvable setup failed " + ex);
        }
    }

    public void Approve()
    {
        if (DatabaseManager.TryApproveProduct(_self))
            Destroy(gameObject);
        else
            Debug.LogError("Product approval failed");
    }

    public void Disapprove()
    {
        if (DatabaseManager.TryDisapproveProduct(_self))
            Destroy(gameObject);
        else
            Debug.LogError("Product disapproval failed");
    }

}
