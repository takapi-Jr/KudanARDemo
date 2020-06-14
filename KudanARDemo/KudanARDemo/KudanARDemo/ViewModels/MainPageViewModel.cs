using KudanARDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KudanARDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand MarkerARCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand MarkerLessARCommand { get; } = new AsyncReactiveCommand();


        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            MarkerARCommand.Subscribe(async () =>
            {
                var dependencyService = new Prism.Services.DependencyService();
                await dependencyService.Get<IKudanARManager>().StartMarkerARActivityAsync();
            });

            MarkerLessARCommand.Subscribe(async () =>
            {
                //var dependencyService = new Prism.Services.DependencyService();
                //await dependencyService.Get<IKudanARManager>().StartMarkerLessARActivityAsync();
            });
        }
    }
}
