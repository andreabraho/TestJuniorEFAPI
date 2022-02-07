<template>
  <div>


      <table-header :brands="brands" 
                    @selectNewBrand="selectNewBrand"
                    @chageOrder="chageOrder"></table-header>
      
      <table-row v-for="(item,index) in tlist" :key="item.Id" :item="item" :index="index" ></table-row>
      

  </div>
</template>

<script>
import TableHeader from "./TableHeader.vue";
import TableRow from "./TableRow.vue";
export default {
    name:"MyTable",
  data() {
    return {};
  },
  props:{
      tlist:[],
      brands:[]
  },
  methods:{
    selectNewBrand(idBrand){
      this.$emit("selectNewBrand",idBrand)
    },
    chageOrder(orderBy,isAsc){
      this.$emit("chageOrder",orderBy,isAsc)
    }
  },
  components: {
    TableHeader,
    TableRow,
  },

};
</script>

<style>
</style>