using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MonafuwaGlass
{
    public static class QuiltTools
    {
        private const string MenuRoot = "Assets/" + nameof(QuiltTools) + "/";

        private static Texture2D[] 現在選択中のテクスチャを取得()
        {
            return Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        }

        public static Texture2D[] Quilt画像の各タイルを個別のテクスチャに分解(Texture2D quiltTextures, int 横方向のタイル数, int 縦方向のタイル数)
        {
            var タイルの横幅 = quiltTextures.width / 横方向のタイル数;
            var タイルの縦幅 = quiltTextures.height / 縦方向のタイル数;

            // 一番左側の視点から右に向かって順番に分解していく
            // https://docs.lookingglassfactory.com/keyconcepts/quilts
            var tileTextures = new Texture2D[横方向のタイル数 * 縦方向のタイル数];
            var tileIndex = 0;
            for (var y = 0; y < 縦方向のタイル数; y++)
            {
                for (var x = 0; x < 横方向のタイル数; x++)
                {
                    var tileTexture = new Texture2D(タイルの横幅, タイルの縦幅, quiltTextures.format, false);
                    var srcX = x * タイルの横幅;
                    var srcY = y * タイルの縦幅;
                    Graphics.CopyTexture(
                        quiltTextures, 0, 0, srcX, srcY, タイルの横幅, タイルの縦幅,
                        tileTexture, 0, 0, 0, 0);
                    tileTextures[tileIndex++] = tileTexture;
                }
            }

            return tileTextures;
        }

        [MenuItem(MenuRoot + nameof(選択した8x6Quilt画像を分解))]
        private static void 選択した8x6Quilt画像を分解()
        {
            選択したQuilt画像を分解(8, 6);
        }

        private static void 選択したQuilt画像を分解(int 横方向のタイル数, int 縦方向のタイル数)
        {
            foreach (var quiltTexture in 現在選択中のテクスチャを取得())
            {
                // Quilt画像のファイル名でフォルダを掘って各タイル画像をPNGで保存
                var quiltPath = AssetDatabase.GetAssetPath(quiltTexture);
                var tileFolder = Path.Combine(
                    Path.GetDirectoryName(quiltPath), Path.GetFileNameWithoutExtension(quiltPath));
                Directory.CreateDirectory(tileFolder);

                var tileTextures = Quilt画像の各タイルを個別のテクスチャに分解(quiltTexture, 横方向のタイル数, 縦方向のタイル数);
                for (var i = 0; i < tileTextures.Length; i++)
                {
                    var tilePath = $"{tileFolder}/{i}.png";
                    File.WriteAllBytes(tilePath, tileTextures[i].EncodeToPNG());
                    AssetDatabase.ImportAsset(tilePath);

                    // インポート設定
                    var importer = (TextureImporter)AssetImporter.GetAtPath(tilePath);
                    importer.npotScale = TextureImporterNPOTScale.None;
                    importer.mipmapEnabled = false;
                    importer.wrapMode = TextureWrapMode.Clamp;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();

                    // 後始末
                    Object.DestroyImmediate(tileTextures[i]);
                }
            }
        }

        [MenuItem(MenuRoot + nameof(選択した8x6Quilt画像を3Dテクスチャに変換))]
        private static void 選択した8x6Quilt画像を3Dテクスチャに変換()
        {
            選択したQuilt画像を3Dテクスチャに変換(8, 6);
        }

        private static void 選択したQuilt画像を3Dテクスチャに変換(int 横方向のタイル数, int 縦方向のタイル数)
        {
            foreach (var quiltTexture in 現在選択中のテクスチャを取得())
            {
                var tileTextures = Quilt画像の各タイルを個別のテクスチャに分解(quiltTexture, 横方向のタイル数, 縦方向のタイル数);

                // 3Dテクスチャ作成
                var quiltTexture3D = new Texture3D(
                    tileTextures[0].width, tileTextures[0].height, tileTextures.Length, tileTextures[0].format, false);
                quiltTexture3D.wrapMode = TextureWrapMode.Clamp;

                // 3Dテクスチャに各タイルを書き込む
                var タイルのピクセル数 = quiltTexture3D.width * quiltTexture3D.height;
                var pixels = new Color[タイルのピクセル数 * quiltTexture3D.depth];
                for (var i = 0; i < tileTextures.Length; i++)
                {
                    var tilePixels = tileTextures[i].GetPixels();
                    Array.Copy(tilePixels, 0, pixels, i * タイルのピクセル数, タイルのピクセル数);

                    // 後始末
                    Object.DestroyImmediate(tileTextures[i]);
                }
                
                quiltTexture3D.SetPixels(pixels);
                quiltTexture3D.Apply();

                // 保存
                var outputPath = Path.ChangeExtension(AssetDatabase.GetAssetPath(quiltTexture), ".asset");
                AssetDatabase.CreateAsset(quiltTexture3D, outputPath);
            }
        }
    }
}