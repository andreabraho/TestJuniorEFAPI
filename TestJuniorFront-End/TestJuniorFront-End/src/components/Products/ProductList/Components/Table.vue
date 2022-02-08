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
    /**product to be shown */
      tlist:[],
      /**rappresenting the brand for the select */
      brands:[]
  },
  methods:{
    /**method to choose a brand to filter on
    ** @idBrand id of the brand to filter on
    ** launch event selectNewBrand that tells the parent to change filter brand
     */
    selectNewBrand(idBrand){
      this.$emit("selectNewBrand",idBrand)
    },
    /**method to change the order of the list
    ** launch evento changeorder that tells the parent to change order */
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