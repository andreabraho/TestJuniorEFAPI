<template>
  <div id="app" class="container-fluid">

    <div class="row sticky-top" ><nav class="navbar navbar-light bg-dark mynav ">
        <span class="navbar-brand text-light ml-2">Company name</span>
    </nav></div>
    
    

    <div class="row ">

    <div class="col-2 h-100 position-fixed sidebar " >
      <div class="mt-2">
            <router-link to="/Products" class="sidebarComp mt-3 " >
            <span class="mt-3" 
                  :class="[activeLink==1?'active':'']">
              Prodotti</span> </router-link><br>
      </div>
      <div class="mt-2">
            <router-link to="/Brands" class="sidebarComp mt-3" >
            <span class="mt-3"
                  :class="[activeLink==2?'active':'']">
              Brands</span></router-link><br>
      </div>
      <div class="mt-2">
            <router-link to="/leads" class="sidebarComp mt-3" >
            <span class="mt-3"
                  :class="[activeLink==3?'active':'']"
                  >
              Richiete info</span></router-link><br>
      </div>
     
    </div>

    <div class="col-10 offset-2 myoffset">
      <router-view 
          @setActiveLink="setActiveLink"></router-view>
    </div>
    </div>
    
  </div>
</template>

<script>

export default {
  name: "App",
  components: {},
  data(){
    return {
      /**1(product)2(brand)3(leads) set the active link on data send from child component */
      activeLink:0,
      myvar:1
    }
  },
  methods:{
    /**sets active link launched from child components */
    setActiveLink(num){
      this.activeLink=num
    }
  },
  /**default active link */
  created(){
    this.activeLink=0
  }
};
</script>

<style >
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
.sidebar{
  background-color: rgb(241, 239, 239);
}
.sidebarComp{
  margin-top: 3rem!important;
  color: gray;
}
.sidebarCompa:link {
    text-decoration: none;
}
a {
    text-decoration: none!important;
}
a:hover {
    text-decoration: underline;
    color: rgb(109, 98, 255);
}
.active {
    text-decoration: none;
    color: rgb(17, 0, 255);
}
.mw-10{
        max-width: 10%;
    }
.mw-40{
    max-width: 40%;
}
.mw-30{
    max-width: 30%;
}
.mynav{
  max-width: 100%;
}
.myoffset{
  offset-position: auto;
}
.p-0{
  padding: 0px 0px 0px 24px !important;

}
</style>
