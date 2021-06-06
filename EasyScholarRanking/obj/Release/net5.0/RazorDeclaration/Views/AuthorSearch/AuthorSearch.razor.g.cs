// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace EasyScholarRanking.Views.AuthorSearch
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Jorge\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\AuthorSearch.razor"
using EasyScholarRanking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Jorge\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\AuthorSearch.razor"
using EasyScholarRanking.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Jorge\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\AuthorSearch.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    public partial class AuthorSearch : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 175 "C:\Jorge\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\AuthorSearch.razor"
       

    //////////////////////////////////////////////////////////////////////////////////////////////////////// PROPERTIES

    // Form searching properties
    private string Venue = "";
    private string Location = "International";
    private int MinYear = 2019;
    private int MaxYear = 2020;
    private string Author = "";
    private string LocationSearch = "";

    // Search variables
    private List<Venue> venueList;
    private List<AuthorClass> googleAuthorList;
    private List<AuthorClass> venueAuthorList;
    private List<String> locationList;

    private string PositionVenue = "";
    private string PublicationsVenue = "";
    private string ActiveYearsVenue = "";
    private string PublicationMeanVenue = "";
    private string PercentileVenue = "";
    private string QuartileVenue = "";
    private string VenueKey = "";

    private string PositionGoogle = "";
    private string PublicationsGoogle = "";
    private string ActiveYearsGoogle = "";
    private string PublicationMeanGoogle = "";
    private string NumberVenuesGoogle = "";
    private string PercentileGoogle = "";
    private string QuartileGoogle = "";


    // UI properties
    private bool IsLoading = false;
    private bool ToggleFormValue = false;
    private bool AuthorFound = true;
    private bool GoogleAuthorFound = true;
    private string Query;
    private string IndividualVenueTitle = "Single";


    //////////////////////////////////////////////////////////////////////////////////////////////////////// FUNCTIONS

    //
    // Search and set author info
    private async void SearchAuthor()
    {
        if (!string.IsNullOrWhiteSpace(Venue) && MinYear > 0 && MaxYear > 0 && !string.IsNullOrWhiteSpace(Author) && !string.IsNullOrWhiteSpace(Location))
        {
            // Change UI
            IsLoading = true;
            ToggleForm();

            await Task.Run(SearchVenues);

            // Update UI
            IsLoading = false;
        }

        Query = DataHelper.GetAuthorInfoQuery(Venue, Location, MinYear, MaxYear, Author);

        // Tell application we have changed the UI, after awaiting the ranking it does not update automatically
        this.StateHasChanged();
    }

    private async void SearchVenues()
    {
        if (!Venue.Equals("Google Scholar Ranking (Software Systems) - March 2021"))
        {
            SearchIndividualVenue();
            SearchGoogleScholar(false);
        }
        else
        {
            SearchGoogleScholar(true);
            SetBestIndividualVenue();
        }
    }

    // Search Individual Venue and set properties
    private void SearchIndividualVenue()
    {
        venueAuthorList = new List<AuthorClass>();
        venueAuthorList = Data.SearchDblp.GetIndividualVenueRanking(DataHelper.ParseVenue(Venue, venueList), MinYear, MaxYear, Location, null, LocationSearch);

        // Get author object given the name
        AuthorClass authorClass = DataHelper.GetAuthorByName(Author, venueAuthorList);
        if (authorClass == null)
        {
            AuthorNotFound();
            AuthorFound = false;
        }
        else
        {
            SetAuthorProperties(authorClass);
            AuthorFound = true;
        }
    }

    // Search Google Scholar Ranking
    private void SearchGoogleScholar(bool searchBest)
    {
        googleAuthorList = new List<AuthorClass>();
        if (searchBest)
        {
            IndividualVenueTitle = "Best";
            googleAuthorList = Data.SearchDblp.GetIndividualVenueRanking("GSR", MinYear, MaxYear, Location, Author, LocationSearch);
        }
        else
        {
            IndividualVenueTitle = "Single";
            googleAuthorList = Data.SearchDblp.GetIndividualVenueRanking("GSR", MinYear, MaxYear, Location, null, LocationSearch);
        }


        // Get author object given the name
        AuthorClass authorClass = DataHelper.GetAuthorByName(Author, googleAuthorList);
        if (authorClass == null)
        {
            GoogleAuthorNotFound();
            GoogleAuthorFound = false;

        }
        else
        {
            SetGoogleAuthorProperties(authorClass);
            GoogleAuthorFound = true;
        }
    }

    // Set properties to empty when author can't be found in a ranking
    private void AuthorNotFound()
    {
        PositionVenue = "";
        PublicationsVenue = "";
        ActiveYearsVenue = "";
        PublicationMeanVenue = "";
        VenueKey = "";
        PercentileVenue = "";
        QuartileVenue = "";
    }

    // Set properties to empty when author can't be found in google ranking
    private void GoogleAuthorNotFound()
    {
        PositionGoogle = "";
        PublicationsGoogle = "";
        ActiveYearsGoogle = "";
        PublicationMeanGoogle = "";
        NumberVenuesGoogle = "";
        PercentileGoogle = "";
        QuartileGoogle = "";
    }

    // Find and set properties for an Author
    private void SetAuthorProperties(AuthorClass authorClass)
    {
        PositionVenue = authorClass.Position.ToString() + " / " + venueAuthorList.Count.ToString();
        PublicationsVenue = authorClass.Score.ToString() + " publications";
        ActiveYearsVenue = (authorClass.GetYearDifference() + 1).ToString() + " years";
        PublicationMeanVenue = DataHelper.GetMean(authorClass.Score, authorClass.GetYearDifference() + 1).ToString() + " publications by year";
        VenueKey = DataHelper.ParseVenue(Venue, venueList);
        int percentile = DataHelper.GetPercentile(authorClass.Score, venueAuthorList);
        PercentileVenue = percentile.ToString() + "th";
        QuartileVenue = DataHelper.GetQuartile(percentile);
    }

    // Find and set properties for an Author inside the google ranking
    private void SetGoogleAuthorProperties(AuthorClass authorClass)
    {
        PositionGoogle = authorClass.Position.ToString() + " / " + googleAuthorList.Count.ToString();
        PublicationsGoogle = authorClass.Score.ToString() + " publications";
        ActiveYearsGoogle = (authorClass.GetYearDifference() + 1).ToString() + " years";
        PublicationMeanGoogle = DataHelper.GetMean(authorClass.Score, authorClass.GetYearDifference() + 1).ToString() + " publications by year";
        NumberVenuesGoogle = "published in " + authorClass.VenueList.Count.ToString() + " / 20 GSR venues";
        int percentile = DataHelper.GetPercentile(authorClass.Score, googleAuthorList);
        PercentileGoogle = percentile.ToString() + "th";
        QuartileGoogle = DataHelper.GetQuartile(percentile);
    }

    // Set best individual venue
    private void SetBestIndividualVenue()
    {
        // Obtain best venue
        AuthorClass bestVenue = DataHelper.GetAuthorByName("BestVenue", googleAuthorList);

        if (bestVenue != null)
        {
            // Set Best Venue to current venue and search individually
            Venue = bestVenue.VenueList.First();
            SearchIndividualVenue();

            // Set Venue again to GSR so user can keep looking in the same venue it wanted
            Venue = "Google Scholar Ranking (Software Systems) - March 2021";
            AuthorFound = true;
        }
        else
        {
            AuthorNotFound();
            AuthorFound = false;
        }
    }

    // Toggle Searching Form
    private void ToggleForm()
    {
        ToggleFormValue = !ToggleFormValue;
    }

    // Change Location Search Type
    private void SetLocationSearch(string locationSearch)
    {
        LocationSearch = locationSearch;
    }

    // Get venue list from json
    protected override void OnInitialized()
    {
        venueList = Data.JsonFileVenueService.GetVenues().ToList();
        locationList = DataHelper.GetLocalSearchLocations();
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
