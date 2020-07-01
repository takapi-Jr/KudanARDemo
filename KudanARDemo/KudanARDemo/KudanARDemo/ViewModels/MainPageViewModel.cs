using KudanARDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
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

namespace KudanARDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        public static readonly string MarkerStr = "Marker";
        public static readonly string NodeStr = "Node";
        public static readonly string ImageMarkerFileName = "Kudan_Lego_Marker.jpg";
        public static readonly string ImageNodeFileName = "Kudan_Cow.png";

        public static ReactiveProperty<ImageInfo> ImageMarkerInfo { get; } = new ReactiveProperty<ImageInfo>(new ImageInfo { ImagePath = ImageMarkerFileName, IsAsset = true });
        public static ReactiveProperty<ImageInfo> ImageNodeInfo { get; } = new ReactiveProperty<ImageInfo>(new ImageInfo { ImagePath = ImageNodeFileName, IsAsset = true });

        public ReactiveProperty<string> ImageMarkerPath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ImageNodePath { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand<string> ChangeImageCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand<string> TakePhotoCommand { get; } = new AsyncReactiveCommand<string>();
        public AsyncReactiveCommand MarkerARCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand MarkerlessARCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand MarkerlessWallCommand { get; } = new AsyncReactiveCommand();
        
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "HOME";

            ImageMarkerInfo.Subscribe(imageInfo =>
            {
                ImageMarkerPath.Value = imageInfo.ImagePath;
            }).AddTo(this.Disposable);

            ImageNodeInfo.Subscribe(imageInfo =>
            {
                ImageNodePath.Value = imageInfo.ImagePath;
            }).AddTo(this.Disposable);

            ChangeImageCommand.Subscribe(async (param) =>
            {
                var imagePath = await Common.GetImagePath();
                var imageInfo = new ImageInfo();

                if (param.Contains(MarkerStr))
                {
                    // 前回指定したマーカー画像の一時ファイルを削除
                    if (!ImageMarkerInfo.Value.IsAsset)
                    {
                        File.Delete(ImageMarkerInfo.Value.ImagePath);
                    }

                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageMarkerFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageMarkerInfo.Value = imageInfo;
                }
                else if(param.Contains(NodeStr))
                {
                    // 前回指定したノード画像の一時ファイルを削除
                    if (!ImageNodeInfo.Value.IsAsset)
                    {
                        File.Delete(ImageNodeInfo.Value.ImagePath);
                    }

                    imageInfo.ImagePath = File.Exists(imagePath) ? imagePath : ImageNodeFileName;
                    imageInfo.IsAsset = !File.Exists(imagePath);
                    ImageNodeInfo.Value = imageInfo;
                }
            }).AddTo(this.Disposable);

            TakePhotoCommand.Subscribe(async (param) =>
            {
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
            }).AddTo(this.Disposable);

            MarkerARCommand.Subscribe(async () =>
            {
                var dependencyService = new Prism.Services.DependencyService();
                await dependencyService.Get<IKudanARService>().StartMarkerARActivityAsync();
            }).AddTo(this.Disposable);

            MarkerlessARCommand.Subscribe(async () =>
            {
                var dependencyService = new Prism.Services.DependencyService();
                await dependencyService.Get<IKudanARService>().StartMarkerlessARActivityAsync();
            }).AddTo(this.Disposable);

            MarkerlessWallCommand.Subscribe(async () =>
            {
                var dependencyService = new Prism.Services.DependencyService();
                await dependencyService.Get<IKudanARService>().StartMarkerlessWallActivityAsync();
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
