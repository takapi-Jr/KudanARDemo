using KudanARDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReactiveProperty<ObservableCollection<KudanARKind>> ARKindList { get; } = new ReactiveProperty<ObservableCollection<KudanARKind>>(new ObservableCollection<KudanARKind>()
            {
                new KudanARKind
                {
                    Name = "マーカーAR",
                    Image = "MarkerAR_Sample.jpg",
                    CommandParameter = "MarkerAR",
                },
                new KudanARKind
                {
                    Name = "マーカーレスAR(フロアトラッキング)",
                    Image = "MarkerlessAR_Floor_Sample.jpg",
                    CommandParameter = "MarkerlessAR_Floor",
                },
                new KudanARKind
                {
                    Name = "マーカーレスAR(壁トラッキング)",
                    Image = "MarkerlessAR_Wall_Sample.jpg",
                    CommandParameter = "MarkerlessAR_Wall",
                },
            });

        public AsyncReactiveCommand<string> ChangeImageCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand<string> TakePhotoCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand<string> ExecuteARCommand { get; } = new AsyncReactiveCommand<string>();
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

                if (param.Equals(MarkerStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageMarkerFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageMarkerInfo.Value = imageInfo;
                }
                else if (param.Equals(NodeStr))
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

                if (param.Equals(MarkerStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageMarkerFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageMarkerInfo.Value = imageInfo;
                }
                else if (param.Equals(NodeStr))
                {
                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageNodeFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageNodeInfo.Value = imageInfo;
                }
                IsBusy.Value = false;
            }).AddTo(this.Disposable);

            ExecuteARCommand = IsBusy.Inverse().ToAsyncReactiveCommand<string>();
            ExecuteARCommand.Subscribe(async (param) =>
            {
                if (param.Equals("MarkerAR"))
                {
                    IsBusy.Value = true;
                    await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerARActivityAsync();
                }
                else if (param.Equals("MarkerlessAR_Floor"))
                {
                    IsBusy.Value = true;
                    await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerlessARActivityAsync();
                }
                else if (param.Equals("MarkerlessAR_Wall"))
                {
                    IsBusy.Value = true;
                    await Xamarin.Forms.DependencyService.Get<IKudanARService>().StartMarkerlessWallActivityAsync();
                }
                // Activity側でビジーフラグ変更
                //IsBusy.Value = false;
            }).AddTo(this.Disposable);

            SettingCommand = IsBusy.Inverse().ToAsyncReactiveCommand();
            SettingCommand.Subscribe(async () =>
            {
                await this.NavigationService.NavigateAsync("SettingPage");
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
