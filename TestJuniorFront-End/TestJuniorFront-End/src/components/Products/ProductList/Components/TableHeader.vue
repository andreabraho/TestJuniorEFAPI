<template>
  <div>
    <div class="row ">
      <div  class="col-2 mw-10 position-relative orderbox mh-36 " 
            @click="changeOrder(1)"

            >
            <b>Brand</b>
      
      
        <i class="bi bi-caret-up-fill position-absolute top-0 end-0 i-g"
            :class="[orderBy==1 && isAsc==false?'i-b':'i-g']"></i>
        <i class="bi bi-caret-down-fill position-absolute bottom-0 end-0 i-g"
        :class="[orderBy==1 && isAsc?'i-b':'i-g']"></i>
        
      
      
      </div>
      <div  class="col-5 mw-40 position-relative orderbox" 
            @click="changeOrder(2)"
            
            >
            <b>Prodotto</b>
      
        <i class="bi bi-caret-up-fill position-absolute top-0 end-0 "
            :class="[orderBy==2 && isAsc==false?'i-b':'i-g']"></i>
        <i class="bi bi-caret-down-fill position-absolute bottom-0 end-0 "
            :class="[orderBy==2 && isAsc?'i-b':'i-g']"></i>
      
      </div>
      
      <div class="col-3 mw-30"><b>Categoria</b></div>

      <div  class="col-1 mw-10 position-relative orderbox" 
            @click="changeOrder(3)"

            ><b>Prezzo</b>
      
        <i class="bi bi-caret-up-fill position-absolute top-0 end-0 i-g"
            :class="[orderBy==3 && isAsc==false?'i-b':'i-g']"></i>
        <i class="bi bi-caret-down-fill position-absolute bottom-0 end-0 i-g"
            :class="[orderBy==3 && isAsc?'i-b':'i-g']"></i>

      </div>
      <div class="col-1 mw-10"></div>
    </div>
    <div class="row  bg-grey t-header">
      <div class="col-2 mw-10 mb-2 mt-2" >

        <select class="form-select" 
                @change="changeBrand()" 
                v-model="selectedBrand">
          <option default value="0">Tutti i Brand</option>

          <option   v-for="brand in brands" 
                    :key="brand.id" 
                    :value="brand.id">{{brand.name}}</option>
          
        </select>
      </div>
      <div class="col-5 mw-40">

      </div>
      <div class="col-3 mw-30"></div>
      <div class="col-1 mw-10"></div>
      <div class="col-1 mw-10"></div>
    </div>
  </div>
</template>



<script>
export default {
  name: "TableHeader",
  props:{
      brands:[]
  },
  data(){
      return{
          selectedBrand:0,
          orderBy:0,
          isAsc:false
      }
  },
  methods:{
      changeBrand(){
          this.$emit("selectNewBrand",this.selectedBrand)
      },
      changeOrder(orderBy){
          this.orderBy=orderBy
          this.isAsc=!this.isAsc
          this.$emit("chageOrder",this.orderBy,this.isAsc)
      }
  }
};
</script>

<style scoped>
    .bg-grey{
        background-color: rgb(230, 230, 230);
    }
    .t-header{
        border-bottom: 3px solid black;
    }
    .mh-36{
        min-height: 36px;
    }
    .i-g{
        color: rgb(209, 208, 208);
        font-size: 15px;
    }
    .i-b{
        color: rgb(46, 45, 45);
    }
    .orderbox:hover{
        background-color: rgb(255, 255, 255);
        cursor: pointer;
    }
    

</style>