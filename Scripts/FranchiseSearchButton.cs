namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FranchiseSearchButton : MonoBehaviour
    {
        public string franchise;

        public SearchBar searchBar;

        private void Awake()
        {
            if(searchBar == null)
            {
                searchBar = GetComponentInParent<SearchBar>();
            }
        }

        public void ButtonClick()
        {
            searchBar.SearchActivityByText(franchise.ToString());
        }
    }
}