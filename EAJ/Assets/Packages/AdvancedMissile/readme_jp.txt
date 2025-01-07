<<バージョン>>
3.2.1

<<更新情報>>
1.1
・ミサイルオブジェクトの追加
・バーナーエフェクトの追加
・エフェクト項目内の「Offset」項目を追加

1.2.1
・「Movement」項目に移動方向を指定できる「Direction」項目を追加
・一部項目名の変更
・一部サンプルのミサイルの値を更新

2.2.1
・一部項目名の変更
・usingステートメントに#if UNITY_EDITOR プリプロセッサ ディレクティブを追加していなかったのを修正
・経路探索による障害物回避機能を追加(項目：Avoidance)
・高速移動によるコリジョン抜け対策のための機能を追加(項目：Collision > Enable Liner Collider)
・RigidBodyに関する設定項目を追加

3.0.0
・一部項目名の変更
・一部処理の変更
・全体的なUIの変更
・仕様を大幅変更。実行させたい処理のみをチェックボックスで選択できるように
・信管機能を追加(項目：Fuse)
・障害物回避機能に、左右だけでなく上下方向にも回避ルートを生成するよう修正
・DEMOの変更

<<使用方法>>
1. ミサイルにしたいオブジェクトに「AdvancedMissile.cs」をアタッチする。
2. 各パラメータを選択・調整し、ミサイルの挙動を作成する。
3. Unityを実行することで、実際の動作を確認する。

<<パラメータ>>
・Destroy
ミサイルの破壊に関する設定を行う。

	- MinDestroyTime
	ミサイル破壊までの最小時間。

	- MaxDestroyTime
	ミサイル破壊までの最大時間。

	- LowPowerFall
	自然落下するかどうか。

	-- FallStartTime
	落下開始までの経過時間（秒）。

	-- Drag
	落下する際の空気抵抗（大きいほど落下が遅くなる）。
	RigidBodyに影響を及ぼす。

・Fuse
信管に関する設定を行う。

	-FuseType
	信管種類の設定

	--SuperQuick
	瞬発信管。接触直後に爆破
	
	--Time
	時限信管。設定時間後に爆破

	---Time
	爆破するまでの時間

	--Proximity
	近接信管。目標との距離で爆破

	---Distance
	爆破する距離

	--Height
	高度信管

	---HeightType
	高度を検知する方法

	----Position
	座標で検知

	----Raycast
	レイキャストで検知

	-----LayerMask
	レイキャストを検知させないレイヤー指定

	-----Height
	爆破させる高度

・Movement
ミサイルの移動についての設定を行う。

	- AnglePerSecond
	1秒間に回転する角度。
	
	- Type
	移動方法の指定。

	- Direction
	移動方向の指定。

	-- Translate
	座標移動。

	--- MinSpeed
	最小速度。

	--- MaxSpeed
	最大速度。

	-- Addforce
	物理移動。

	--- ForceMode
	力のかけ方の指定。

	--- MinPower
	最小力。

	--- MaxPower
	最大力。

	-RotateType
	回転方法の指定

	--NoRotate
	回転させない

	--PerSecond
	度/秒で回転

	---Angle
	1秒で回転させる角度

	--Torque
	Torqueで回転

	---Torque
	加えるトルク値

・Search
目標の検索についての設定を行う。

	- ActiveFollowInterval
	目標への追従を開始するまでの時間（秒）。

	- SerachType
	目標の検索方法。

	-- Element
	目標とするタグもしくは名前一覧。

	- CanReseach
	追従中、目標を再検索するかどうか。
	最も近いものが選ばれる。

	-- Interval
	再検索する間隔（秒）。

	- SightAngle
	ミサイルの持つ視界。
	視界外に出た場合、ミサイルはひたすら前方へ飛び続ける。
	左右をX、上下をYで指定する。

・Offset
目標位置のズレについての設定を行う。

	- Offset
	目標座標をずらす。
	プレイヤーの足元でなく、腰辺りを目標にしたい場合などに用いる。

	- ActiveRandomOffset
	目標座標をランダムにするかどうか。

	-- Amplitude
	目標とのズレ幅。

	-- OffsetX, Y, Z
	目標との距離に応じたズレ具合（0～1）。
	・横軸：距離
	・縦軸：ズレ具合

	-- MinInterval
	ズレ幅を再計算するまでの最小時間（秒）。

	-- MaxInterval
	ズレ幅を再計算するまでの最大時間（秒）。

・Collision
ミサイルのあたり判定についての設定を行う。

	- EnableCollision
	あたり判定を有効にするかどうか。

	-- EnableInterval
	あたり判定を有効にするまでの時間（秒）。

	-- ColliderEachOther
	ミサイル同士を衝突可能にするかどうか。

	--EnableLinerCollider
	ラインコライダを有効にするか

	---Radius
	カプセルレイの半径

・Avoidance
障害物回避についての設定を行う。

	-CanAvoid
	障害物回避を有効にするか。

	--DistanceBetweenObstacle
	障害物からどれだけ離れるか。

	--ReCreateInterval
	経路再生成の間隔（秒）。

	--CreateUpRoute
	上方向ルートを生成するかどうか。
	
	--CreateDownRoute
	下方向ルートを生成するかどうか。

	--DrawRoute
	ルートをシーンビューに描画するかどうか。

・Audios
効果音についての設定を行う。

	- ShotSE
	ミサイル発射時の効果音。

	-- Volume
	ミサイル発射時の効果音音量。

	- ExplosionSE
	ミサイル爆発時の効果音。

	-- Volume
	ミサイル爆発時の効果音音量。

・Effects
エフェクトについての設定を行う。

	- SmokeEffect
	ミサイルから出る煙エフェクト（ビームでも可）。

	--Offset
	エフェクトを発生させる座標をずらす。

	-- KeepChild
	ミサイルが破壊される際、エフェクトをミサイルの子の状態で維持するかどうか。
	維持する場合、ミサイル破壊と同時にエフェクトも消える。

	--- DestroyTime
	エフェクトを削除するまでの時間（秒）。

	- ExplosionEffect
	爆発エフェクトオブジェクト。

	- LocalScale
	エフェクトのローカルスケール。

・Event
ミサイルが目標へ衝突した際に呼ばれるイベントの設定を行う。

	- CallMethod
	呼び出す関数名。

	- SendValueType
	関数に渡す引数の型。

	-- Value
	関数に渡す引数の値。

<<デモ>>
https://www.youtube.com/watch?v=CLC2xMn3a4s

<<連絡>>
Twitter:@isemito_niko