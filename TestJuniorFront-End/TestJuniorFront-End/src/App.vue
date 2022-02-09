<template>
  <div id="app" >
    <nav class="navbar navbar-light bg-dark sticky-top">
        <span class="navbar-brand text-light ml-2">Company name</span>
    </nav>
    

    <div class="row ">
    <div class="col-2 h-100 position-fixed sidebar" >
      <div class="mt-2">
            <router-link to="/Products" class="sidebarComp mt-3 " ><span class="mt-3">Prodotti</span> </router-link><br>
      </div>
      <div class="mt-2">
            <router-link to="/Brands" class="sidebarComp mt-3" ><span class="mt-3">Brands</span></router-link><br>
      </div>
      <div class="mt-2">
            <router-link to="/leads" class="sidebarComp mt-3" ><span class="mt-3">Richiete info</span></router-link><br>
      </div>
      <div class="mt-2">
            <router-link :to="{ name: 'CreateOrEdit', params: { id: 1 }}" class="sidebarComp mt-3 "><span class="mt-3">TEST:edit prod 1</span></router-link><br>
      </div>
      <div class="mt-2">
            <router-link :to="{ name: 'DetailProduct', params: { id: 1 }}" class="sidebarComp mt-3 "><span class="mt-3">TEST:detail prod 1</span></router-link><br>
      </div>
    </div>

    <div class="col-9 offset-2">
      <router-view></router-view>
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
      activeLink:1
    }
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
</style>
