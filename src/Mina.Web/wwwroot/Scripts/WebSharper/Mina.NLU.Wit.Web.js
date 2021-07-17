(function()
{
 "use strict";
 var Global,WebSharper,Obj,Mina,NLU,Wit,ConverseResponse,Entity,Intent,Meaning,Trait,WitApiResponse,WitApi,IntelliFactory,Runtime,$;
 Global=self;
 WebSharper=Global.WebSharper;
 Obj=WebSharper&&WebSharper.Obj;
 Mina=Global.Mina=Global.Mina||{};
 NLU=Mina.NLU=Mina.NLU||{};
 Wit=NLU.Wit=NLU.Wit||{};
 ConverseResponse=Wit.ConverseResponse=Wit.ConverseResponse||{};
 Entity=Wit.Entity=Wit.Entity||{};
 Intent=Wit.Intent=Wit.Intent||{};
 Meaning=Wit.Meaning=Wit.Meaning||{};
 Trait=Wit.Trait=Wit.Trait||{};
 WitApiResponse=Wit.WitApiResponse=Wit.WitApiResponse||{};
 WitApi=Wit.WitApi=Wit.WitApi||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 $=Global.jQuery;
 ConverseResponse=Wit.ConverseResponse=Runtime.Class({
  set_confidence:function(value)
  {
   this.$confidence=value;
  },
  get_confidence:function()
  {
   return this.$confidence;
  },
  set_entities:function(value)
  {
   this.$entities=value;
  },
  get_entities:function()
  {
   return this.$entities;
  },
  set_action:function(value)
  {
   this.$action=value;
  },
  get_action:function()
  {
   return this.$action;
  },
  set_msg:function(value)
  {
   this.$msg=value;
  },
  get_msg:function()
  {
   return this.$msg;
  },
  set_type:function(value)
  {
   this.$type=value;
  },
  get_type:function()
  {
   return this.$type;
  },
  $init:function()
  {
   this.set_type(null);
   this.set_msg(null);
   this.set_action(null);
   this.set_entities(null);
   this.set_confidence(0);
  }
 },Obj,ConverseResponse);
 ConverseResponse.New=Runtime.Ctor(function()
 {
  this.$init();
 },ConverseResponse);
 Entity=Wit.Entity=Runtime.Class({
  set_type:function(value)
  {
   this.$type=value;
  },
  get_type:function()
  {
   return this.$type;
  },
  set_value:function(value)
  {
   this.$value=value;
  },
  get_value:function()
  {
   return this.$value;
  },
  set_suggested:function(value)
  {
   this.$suggested=value;
  },
  get_suggested:function()
  {
   return this.$suggested;
  },
  set_entities:function(value)
  {
   this.$entities=value;
  },
  get_entities:function()
  {
   return this.$entities;
  },
  set_confidence:function(value)
  {
   this.$confidence=value;
  },
  get_confidence:function()
  {
   return this.$confidence;
  },
  set_body:function(value)
  {
   this.$body=value;
  },
  get_body:function()
  {
   return this.$body;
  },
  set_end:function(value)
  {
   this.$end=value;
  },
  get_end:function()
  {
   return this.$end;
  },
  set_start:function(value)
  {
   this.$start=value;
  },
  get_start:function()
  {
   return this.$start;
  },
  set_role:function(value)
  {
   this.$role=value;
  },
  get_role:function()
  {
   return this.$role;
  },
  set_name:function(value)
  {
   this.$name=value;
  },
  get_name:function()
  {
   return this.$name;
  },
  set_id:function(value)
  {
   this.$id=value;
  },
  get_id:function()
  {
   return this.$id;
  },
  $init:function()
  {
   this.set_id(null);
   this.set_name(null);
   this.set_role(null);
   this.set_start(0);
   this.set_end(0);
   this.set_body(null);
   this.set_confidence(0);
   this.set_entities(null);
   this.set_suggested(null);
   this.set_value(null);
   this.set_type(null);
  }
 },Obj,Entity);
 Entity.New=Runtime.Ctor(function()
 {
  this.$init();
 },Entity);
 Intent=Wit.Intent=Runtime.Class({
  set_confidence:function(value)
  {
   this.$confidence=value;
  },
  get_confidence:function()
  {
   return this.$confidence;
  },
  set_name:function(value)
  {
   this.$name=value;
  },
  get_name:function()
  {
   return this.$name;
  },
  set_id:function(value)
  {
   this.$id=value;
  },
  get_id:function()
  {
   return this.$id;
  },
  $init:function()
  {
   this.set_id(null);
   this.set_name(null);
   this.set_confidence(0);
  }
 },Obj,Intent);
 Intent.New=Runtime.Ctor(function(_id,_name,_confidence)
 {
  this.$init();
  this.set_id(_id);
  this.set_name(_name);
  this.set_confidence(_confidence);
 },Intent);
 Meaning=Wit.Meaning=Runtime.Class({
  set_traits:function(value)
  {
   this.$traits=value;
  },
  get_traits:function()
  {
   return this.$traits;
  },
  set_entities:function(value)
  {
   this.$entities=value;
  },
  get_entities:function()
  {
   return this.$entities;
  },
  set_intents:function(value)
  {
   this.$intents=value;
  },
  get_intents:function()
  {
   return this.$intents;
  },
  set_text:function(value)
  {
   this.$text=value;
  },
  get_text:function()
  {
   return this.$text;
  },
  $init:function()
  {
   this.set_text(null);
   this.set_intents(null);
   this.set_entities(null);
   this.set_traits(null);
  }
 },Obj,Meaning);
 Meaning.New=Runtime.Ctor(function()
 {
  this.$init();
 },Meaning);
 Trait=Wit.Trait=Runtime.Class({
  set_confidence:function(value)
  {
   this.$confidence=value;
  },
  get_confidence:function()
  {
   return this.$confidence;
  },
  set_value:function(value)
  {
   this.$value=value;
  },
  get_value:function()
  {
   return this.$value;
  },
  set_id:function(value)
  {
   this.$id=value;
  },
  get_id:function()
  {
   return this.$id;
  },
  $init:function()
  {
   this.set_id(null);
   this.set_value(null);
   this.set_confidence(0);
  }
 },Obj,Trait);
 Trait.New=Runtime.Ctor(function()
 {
  this.$init();
 },Trait);
 WitApiResponse=Wit.WitApiResponse=Runtime.Class({
  set_code:function(value)
  {
   this.$code=value;
  },
  get_code:function()
  {
   return this.$code;
  },
  set_error:function(value)
  {
   this.$error=value;
  },
  get_error:function()
  {
   return this.$error;
  },
  $init:function()
  {
   this.set_error(null);
   this.set_code(null);
  }
 },Obj,WitApiResponse);
 WitApiResponse.New=Runtime.Ctor(function()
 {
  this.$init();
 },WitApiResponse);
 WitApi=Wit.WitApi=Runtime.Class({
  getMeaning:function(sentence,success,error)
  {
   var $this,$1,$2,$3,$4,$5,$6,$7,$8;
   $this=this;
   $.ajax(($1={},($1.url=$2="https://api.wit.ai/message?q="+sentence,$1.beforeSend=$3=function(jqxhr)
   {
    return jqxhr.setRequestHeader("Authorization",$this.authValue);
   },$1.type=$4="GET",$1.contentType=$5="application/json",$1.dataType=$6="json",$1.success=$7=success,$1.error=$8=error,$1)));
  },
  $init:function()
  {
   this.authValue=null;
  }
 },Obj,WitApi);
 WitApi.New=Runtime.Ctor(function(token)
 {
  this.$init();
  this.authValue="Bearer "+token;
 },WitApi);
}());
