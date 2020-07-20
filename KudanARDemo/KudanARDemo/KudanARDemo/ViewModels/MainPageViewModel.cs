using KudanARDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KudanARDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        public static readonly string MarkerStr = "Marker";
        public static readonly string NodeStr = "Node";
        public static readonly string ImageMarkerFileName = "Kudan_Lego_Marker.jpg";
        public static readonly string ImageNodeFileName = "Kudan_Cow.png";

        public static ReactiveProperty<bool> IsBusy { get; } = new ReactiveProperty<bool>(false);
        public static ReactiveProperty<ImageInfo> ImageMarkerInfo { get; } = new ReactiveProperty<ImageInfo>(new ImageInfo { ImagePath = ImageMarkerFileName, IsAsset = true });
        public static ReactiveProperty<ImageInfo> ImageNodeInfo { get; } = new ReactiveProperty<ImageInfo>(new ImageInfo { ImagePath = ImageNodeFileName, IsAsset = true });

        public ReactiveProperty<string> ImageMarkerPath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ImageNodePath { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand<string> ChangeImageCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand<string> TakePhotoCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand MarkerARCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand MarkerlessARCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand MarkerlessWallCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SettingCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "HOME";

            ImageMarkerInfo.Subscribe(imageInfo =>
            {
                ImageMarkerPath.Value = null;
                ImageMarkerPath.Value = imageInfo.ImagePath;
            }).AddTo(this.Disposable);

            ImageNodeInfo.Subscribe(imageInfo =>
            {
                ImageNodePath.Value = null;
                ImageNodePath.Value = imageInfo.ImagePath;
            }).AddTo(this.Disposable);

            ChangeImageCommand = IsBusy.Inverse().ToAsyncReactiveCommand<string>();
            ChangeImageCommand.Subscribe(async (param) =>
            {
                IsBusy.Value = true;
                var imagePath = await Common.GetImagePath();
                var imageInfo = new ImageInfo();

                if (param.Contains(MarkerStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageMarkerFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageMarkerInfo.Value = imageInfo;
                }
                else if (param.Contains(NodeStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageNodeFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageNodeInfo.Value = imageInfo;
                }
                IsBusy.Value = false;
            }).AddTo(this.Disposable);

            TakePhotoCommand = IsBusy.Inverse().ToAsyncReactiveCommand<string>();
            TakePhotoCommand.Subscribe(async (param) =>
            {
                IsBusy.Value = true;
                var imagePath = await Common.TakePhoto();
                var imageInfo = new ImageInfo();

                if (param.Contains(MarkerStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageMarkerFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageMarkerInfo.Value = imageInfo;
                }
                else if (param.Contains(NodeStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageNodeFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageNodeInfo.Value = imageInfo;
                }
                IsBusy.Value = false;
            }).AddTo(this.Disposable);

            MarkerARCommand = IsBusy.Inverse().ToAsyncReactiveCommand();
            MarkerARCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerARActivityAsync();
                // Activity側でビジーフラグ変更
                //IsBusy.Value = false;
            }).AddTo(this.Disposable);

            MarkerlessARCommand = IsBusy.Inverse().ToAsyncReactiveCommand();
            MarkerlessARCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerlessARActivityAsync();
                // Activity側でビジーフラグ変更
                //IsBusy.Value = false;
            }).AddTo(this.Disposable);

            MarkerlessWallCommand = IsBusy.Inverse().ToAsyncReactiveCommand();
            MarkerlessWallCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerlessWallActivityAsync();
                // Activity側でビジーフラグ変更
                //IsBusy.Value = false;
            }).AddTo(this.Disposable);

            SettingCommand = IsBusy.Inverse().ToAsyncReactiveCommand();
            SettingCommand.Subscribe(async () =>
            {
                IsBusy.Value = true;
                await this.NavigationService.NavigateAsync("SettingPage");
                IsBusy.Value = false;
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
