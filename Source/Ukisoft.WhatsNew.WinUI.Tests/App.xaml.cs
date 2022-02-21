using System;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace Ukisoft.WhatsNew.WinUI.Tests;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        UnitTestClient.CreateDefaultUI();

        m_window = new MainWindow();
        m_window.Activate();

        UITestMethodAttribute.DispatcherQueue = m_window.DispatcherQueue;
        UnitTestClient.Run(Environment.CommandLine);
    }

    public static Window? m_window;
}
