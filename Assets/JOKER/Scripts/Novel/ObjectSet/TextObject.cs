using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Novel{

	public class TextObject : AbstractObject {

		//private string name;

		private Sprite targetSprite ;
		private bool isShow = false;
		private GuiScaler guiScaler;

		public string filename = "";

		//イメージオブジェクト新規作成
		public TextObject(){
			this.gameManager = NovelSingleton.GameManager;
		}

		public override void init(Dictionary<string,string> param){

			this.param = param;

			GameObject g = Resources.Load(GameSetting.PATH_PREFAB + "Text") as GameObject;
			this.rootObject = (GameObject)Instantiate(g,new Vector3(0,0.5f,-3.2f),Quaternion.identity); 

			GUIText guiText = this.rootObject.GetComponent<GUIText> ().guiText;

			guiText.alignment = TextEnum.textAlignment(this.param["alignment"]);
			guiText.anchor    = TextEnum.textAnchor(this.param["anchor"]);


			string color = this.param ["color"];

			Color objColor =  ColorX.HexToRGB(color);
			objColor.a = 0;
			guiText.color = objColor;

			guiText.fontSize = int.Parse(this.param ["fontsize"]);

			this.guiScaler = new GuiScaler (guiText);
			this.rootObject.name = this.name;

			if (this.param ["layer"] == "ui") {
				//タグをつける
				this.rootObject.tag = "ui";
			}

		}

		public override void set(Dictionary<string,string> param){

			if (this.rootObject == null) {
				this.init (param);
			}

			string text = this.param["val"];

			if (this.param ["cut"] != "") {
				int cut = int.Parse (this.param ["cut"]);
				if (cut < text.Length) {
					text = text.Substring (0,cut);
			
					this.param ["val"] = text;

				}
			}

			this.rootObject.GetComponent<GUIText> ().guiText.text = text;

		}


		public override void setColider(){

			/*
			this.rootObject.AddComponent<BoxCollider2D> ();
			BoxCollider2D b = this.rootObject.GetComponent<BoxCollider2D> ();
			b.isTrigger = true;
			if (this.isShow == true) {
				b.enabled = true;
			} else {
				b.enabled = false;
			}
			Vector2 size = new Vector2 (this.targetSprite.bounds.size.x, this.targetSprite.bounds.size.y);
			b.size = size;
			*/
		}


		public override void show(float time,string easeType){

			this.isShow = true;

			//通常の表示切り替えの場合
			iTween.ValueTo(this.gameObject,iTween.Hash(
				"from",0,
				"to",1,
				"time",time,
				"oncomplete","finishAnimation",
				"oncompletetarget",this.gameObject,
				"easeType",easeType,
				"onupdate","crossFade"
			));



		}

		public override void hide(float time,string easeType){

			this.isShow = false;

			//BoxCollider2D b = this.rootObject.GetComponent<BoxCollider2D> ();
			//b.enabled = false;

			//通常の表示切り替えの場合
			iTween.ValueTo(this.gameObject,iTween.Hash(
				"from",1,
				"to",0,
				"time",time,
				"oncomplete","finishAnimation",
				"oncompletetarget",this.gameObject,
				"easeType",easeType,
				"onupdate","crossFade"
			));

		}



		private void crossFade(float val){

			var color = this.rootObject.GetComponent<GUIText> ().guiText.color;
			color.a = val;
			this.rootObject.GetComponent<GUIText> ().guiText.color = color;

		}



		//アニメーションの終了をいじょうするための
		private void finishAnimation ()
		{

			if (this.completeDeletgate != null) {
				this.completeDeletgate ();
			}

		}


		// Use this for initialization
		void Start () {


		}

		// Update is called once per frame
		void Update () {
			try{
				this.guiScaler.fontResize ();
			}catch(System.Exception e){
				Debug.Log (e.ToString ());
				Destroy (this);
			}
		}

	}


}