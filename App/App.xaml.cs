﻿namespace App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new View.ConnexionUser());
      
    }
}