# 巻乃もなかさんのLooking Glass Portrait用画像をUnityで見る実験

※このリポジトリは [もなふわすい～とる～む Advent Calendar 2021](https://adventar.org/calendars/6449) 12/7(7日目)の記事のために作成したものです。

## はじめに

こんにちは、メタバースエンジニアのこまったくまです。嘘です。

5日目のまめお氏の記事『 [推しをLooking Glass Portraitに召喚する方法](https://papersloth.hatenablog.com/entry/2021/12/05/010857) 』を読んで **「自分も推しを召喚してみたい！」** と思った方は多いと思います。

しかし、私を含む多くのMDC(Monaka Daisuki Club)の方はLooking Glass Portraitを持っていません…。

ぐぬぬ…悔しい…

……

…いや？待てよ…そうか！そうだよっ！！

**Looking Glass PortraitがなければUnityを使えばいいじゃないか！！**

なあそうだろハム太郎！！

ハム太郎「そうなのだ！！！！」

## と、いうわけで、

[こちら](https://www.fanbox.cc/@makinomonaka/posts/2237215) で巻乃もなかさんが公開しているLooking Glass Portrait用画像（※以下Quilt画像）をUnity上の仮想空間…否！**メタバース**で表示する仕組みを実験的に作ってみました。

## 早速やってみよう

### Step 1.

このリポジトリをクローンして、Unity 2020.3.24f1(※この記事を書いている時点での最新版)で開きます。

### Step 2.

[こちら](https://www.fanbox.cc/@makinomonaka/posts/2237215) からお好みのQuilt画像をダウンロードします。

※この説明では **lgp_blackcat8x6** を使用します。

![image](https://user-images.githubusercontent.com/21675144/144872641-40e0258a-c69d-4603-a444-198b7b983f41.png)

### Step 3.

ダウンロードした画像を`Assets/Quilts/`以下に入れます。

![image](https://user-images.githubusercontent.com/21675144/144873765-485e84c1-90de-4b0a-a90a-a9fc11a89e2e.png)

### Step 4.

テクスチャのインポート設定を以下のようにしてください。

![image](https://user-images.githubusercontent.com/21675144/144874226-5eb456e2-f0e9-4034-8dd9-080f3ff98338.png)

プレビュー画像下部に

`3840x3840 (NPOT) RGB8 UNorm 42.2MB`

と表示されればOKです。

### Step 5.

Projectウィンドウでテクスチャを右クリック→`QuiltTools`→`選択した8x6Quilt画像を3Dテクスチャに変換`を選択します。
 
![image](https://user-images.githubusercontent.com/21675144/144876733-578e37a0-f3ce-49e1-85c7-fc78689190fe.png)

成功すると **lgp_blackcat8x6.asset(3Dテクスチャ)** が生成されます。
 
![image](https://user-images.githubusercontent.com/21675144/144877478-f05decf8-ce44-4475-85ee-dee98948b149.png)

### Step 6.

`Assets/Scenes/Demo.unity`を開きます。

### Step 7.

Hierarchyウィンドウの`MonafuwaGlass`というゲームオブジェクトを選択して、Step 5.で生成したlgp_blackcat8x6.assetをオブジェクト中心の灰色の領域にドラッグ＆ドロップします。

![image](https://user-images.githubusercontent.com/21675144/144881616-a926acd5-2440-40ad-8797-898f801948d2.gif)

### Step 8.

本能の赴くままにカメラをグリグリ回して堪能します。

![image](https://user-images.githubusercontent.com/21675144/144882344-e4e95a1a-7575-4cb9-9c54-68a79d677711.gif)

※立体的に見える角度は正面から水平方向に±17.5°なので、傾け過ぎると立体感がなくなります。また、**当然ですが下からは覗けません**。

### Step 9.

飽きたらUnityを閉じます。

### おまけ

多少Unityの知識があれば、たくさん並べて鑑賞することもできます。これぞメタバース！すごいぞメタバース！

![image](https://user-images.githubusercontent.com/21675144/144884601-4232f0a8-aaca-46e2-987e-bd2ff232af6a.gif)

## 技術解説

3DテクスチャにQuilt画像内の各タイル(8 x 6 = 48枚)を左側の視点から順番に書き込んでおき、シェーダで水平方向の傾きに合わせてtex3Dのz座標を計算しているだけです。
（ふいんき計算なので間違ってたら指摘してもらえると助かります！）

## おわりに

いかがでしたか？それではまた来年！
