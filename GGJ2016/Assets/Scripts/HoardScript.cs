using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoardScript : MonoBehaviour {

    public List<GameObject> _hoard = new List<GameObject>();
                                     
	void Start ()
    {              
        for (int i = 0; i < this.transform.childCount; i++)
        {
            _hoard.Add(transform.GetChild(i).gameObject);
        }   
	}   

    public IEnumerator DestroyHoard()
    {
        while (this.transform.childCount > 0)
        {                 
            foreach (GameObject child in _hoard)
            {                
                if (!child.GetComponent<MeshRenderer>().isVisible)
                {      
                    _hoard.Remove(child);  
                    Destroy(child.gameObject);
                    StartCoroutine(DestroyHoard());
                    yield break;
                }
            }   
            yield return new WaitForSeconds(.5f);
        }
    }
}
