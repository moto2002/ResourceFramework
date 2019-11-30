﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ResourceFramework
{
    internal class Resource : AResource
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        internal override void Load()
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException($"{nameof(Resource)}.{nameof(Load)}() {nameof(url)} is null.");

            if (bundle != null)
                throw new Exception($"{nameof(Resource)}.{nameof(Load)}() {nameof(bundle)} not null.");

            string bundleUrl = null;
            if (!ResourceManager.instance.ResourceBunldeDic.TryGetValue(url, out bundleUrl))
                throw new Exception($"{nameof(Resource)}.{nameof(Load)}() {nameof(bundleUrl)} is null.");

            bundle = BundleManager.instance.Load(bundleUrl);
            loadTask = new TaskCompletionSource<AResource>();
            LoadAsset();
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        internal override void UnLoad()
        {
            if (bundle == null)
                throw new Exception($"{ nameof(Resource)}.{nameof(UnLoad)}() {nameof(bundle)} is null.");

            if (asset != null && !(asset is GameObject))
            {
                Resources.UnloadAsset(asset);
                asset = null;
            }

            bundle.ReduceReference();
            bundle = null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        internal override void LoadAsset()
        {
            if (bundle == null)
                throw new Exception($"{nameof(Resource)}.{nameof(LoadAsset)}() {nameof(bundle)} is null.");

            asset = bundle.LoadAsset(url);

            done = true;

            loadTask.SetResult(this);
        }
    }
}