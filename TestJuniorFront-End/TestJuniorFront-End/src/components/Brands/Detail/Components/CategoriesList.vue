<template>
  <div>

      <div class="row box">
          <div class="col-4 mt-3 mb-2 d-flex justify-content-center" 
                v-for="item in categories"
                :key="item.id">
            {{item.name}}
          </div>






      </div>



  </div>
</template>

<script>
export default {
    name:"CategoriesList",
    props:{
        categories:{
            type:[],
            required:true
        }
    },
    data(){
        return {
            coutner:0
        }
    }
}
</script>

<style scoped>
    .box{
        border:2px solid gray;
    }
</style>