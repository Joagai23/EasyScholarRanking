// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace EasyScholarRanking.Views.RankingSearch
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using EasyScholarRanking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using EasyScholarRanking.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using ChartJs.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using ChartJs.Blazor.PieChart;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using ChartJs.Blazor.Common;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    public partial class RankingSearch : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 144 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\RankingSearch\RankingSearch.razor"
       

    //////////////////////////////////////////////////////////////////////////////////////////////////////// PROPERTIES

    // Form searching properties
    private string Venue = "";
    private string Location = "International";
    private int MinYear = 2019;
    private int MaxYear = 2020;
    private int NumberEntries = 8;
    private int CareerLenght = 0;
    private string LocationSearch = "";

    // Search variables
    private List<AuthorClass> authorList;
    private List<Venue> venueList;
    private List<String> locationList;
    private double meanPublicationsTable = 0;

    // Constants
    private double contributionPercentage = 0.015; //1.5%
    private double appereancePercentage = 0.35; //35%

    // UI properties
    private bool IsLoading = false;
    private bool ToggleFormValue = false;
    private PieConfig pieChart;
    private string Query;

    //////////////////////////////////////////////////////////////////////////////////////////////////////// FUNCTIONS

    private async void SetAuthorList()
    {
        if (!string.IsNullOrWhiteSpace(Venue) && MinYear > 0 && MaxYear > 0 && NumberEntries > 0 && CareerLenght >= 0 && !string.IsNullOrWhiteSpace(Location))
        {
            // Change UI
            IsLoading = true;
            ToggleForm();

            // Search ranking and wait until it is returned
            authorList = new List<AuthorClass>(NumberEntries);
            List<AuthorClass> list = await Task.Run(SearchRanking);

            authorList = list;

            // Paint cheese
            if (authorList != null)
            {
                pieChart.Data.Datasets.Clear();
                pieChart.Data.Labels.Clear();
                FillChart();
            }

            // Update UI
            IsLoading = false;
        }

        Query = DataHelper.GetRankingInfoQuery(Venue, Location, MinYear, MaxYear, NumberEntries, CareerLenght);

        // Tell application we have changed the UI, after awaiting the ranking it does not update automatically
        this.StateHasChanged();
    }

    private async Task<List<AuthorClass>> SearchRanking()
    {
        List<AuthorClass> list =  Data.SearchDblp.GetRankingSearch(DataHelper.ParseVenue(Venue, venueList), MinYear, MaxYear, NumberEntries, CareerLenght, Location, null, LocationSearch);

        return list;
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

    // Configure pie chart and get venue list from json
    protected override void OnInitialized()
    {
        pieChart = new PieConfig
        {
            Options = new PieOptions
            {
                Responsive = true,
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = "Authors Pie Chart",
                    FontSize = 16 // Default is 12
                },
                MaintainAspectRatio = false,
                Legend = new Legend
                {
                    Display = false
                }
            }
        };

        venueList = Data.JsonFileVenueService.GetVenues().ToList();
        locationList = DataHelper.GetLocalSearchLocations();
    }

    // Paint cheese wheel if data recovered is valid
    private void FillChart()
    {
        // Init dataset used for cheese wheel
        PieDataset<int> dataset = new PieDataset<int>();

        // Obtain number of authors and publications to calculate the mean
        int totalPublications = DataHelper.GetTotalPublications(authorList);
        int numberAuthors = authorList.Count;

        string meanText = DataHelper.GetMeanText(totalPublications, numberAuthors);
        meanPublicationsTable = DataHelper.GetMean(totalPublications, numberAuthors);

        pieChart.Options.Title.Text = "Authors Pie Chart" + meanText;

        int othersScore = 0;

        // Only paint if author data is valid, if not, paint some text above the wheel
        if (authorList.Count != 0)
        {
            int currentScore = authorList[0].Score;
            int numberOfAuthorsSameScore = DataHelper.CountAuthorByScore(currentScore, authorList);
            double currentScorePercentage = (double)currentScore / (double)totalPublications;
            double currentAppereancePercentage = (double)numberOfAuthorsSameScore / (double)NumberEntries;

            // Given a score get the amount of authors that share it and calculate the percentage among all the authors
            // If the score contributes to a minumum percentage of all the publications it is inserted in the dataset
            // If the number of authors sharing that score is below a minumum percentage it is inserted in the dataset
            foreach (AuthorClass author in authorList)
            {
                if (currentScore != author.Score)
                {
                    currentScore = author.Score;
                    numberOfAuthorsSameScore = DataHelper.CountAuthorByScore(currentScore, authorList);
                    currentScorePercentage = (double)currentScore / (double)totalPublications;
                    currentAppereancePercentage = (double)numberOfAuthorsSameScore / (double)NumberEntries;
                }

                if (currentScorePercentage > contributionPercentage || currentAppereancePercentage < appereancePercentage)
                {
                    double roundedPercentage = Math.Round(currentScorePercentage, 2) * 100;
                    int roundedIntegerPercentage = (int)roundedPercentage;

                    pieChart.Data.Labels.Add(author.Text + " - " + roundedIntegerPercentage + "%");
                    dataset.Add(author.Score);
                }
                else
                {
                    othersScore += author.Score;
                }
            }

            // Calculate values for "Others" and insert them in the dataset
            if (othersScore != 0)
            {
                double percentage = ((double)othersScore / (double)totalPublications) * 100;
                percentage = Math.Round(percentage, 2);

                pieChart.Data.Labels.Add("Others - " + (int)percentage + "%");
                dataset.Add(othersScore);
            }

            dataset.BackgroundColor = DataHelper.GetBackgroundColors(authorList.Count);
            dataset.BorderColor = "#808080";

            pieChart.Data.Datasets.Add(dataset);
        }
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
