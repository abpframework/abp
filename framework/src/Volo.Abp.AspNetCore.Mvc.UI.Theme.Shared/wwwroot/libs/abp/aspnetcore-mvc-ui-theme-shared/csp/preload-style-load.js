$(function (){
    let preLoads = $("link[rel=preload][as=style]");
    preLoads.attr("rel", "stylesheet");
})