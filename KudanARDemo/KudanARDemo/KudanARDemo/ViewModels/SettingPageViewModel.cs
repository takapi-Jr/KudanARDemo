using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Xamarin.Essentials;

namespace KudanARDemo.ViewModels
{
    public class SettingPageViewModel : ViewModelBase, IDisposable
    {
        public ReactiveProperty<string> AppName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand LicenseCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public SettingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "AppInfo";

            // バージョン情報設定
            AppName.Value = AppInfo.Name;
            Version.Value = AppInfo.VersionString;

            LicenseCommand.Subscribe(async () =>
            {
                await this.NavigationService.NavigateAsync("LicensePage");
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
