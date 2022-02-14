<template>
  <div>

      <table-header :brands="brands" 
                    @selectNewBrand="selectNewBrand"
                    @changeOrder="changeOrder"
                    @changeSearch="changeSearch"
                    :searchProp="search"
                    :selectedBrandProp="selectedBrand"
                   ></table-header>
      
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
    /**info request to be shown */
      tlist:[],
      /**rappresenting the brand for the select */
      brands:[],
      search:String,
      selectedBrand:Number
  },
  methods:{
    /**method to choose a brand to filter on
    ** @idBrand id of the brand to filter on
    ** launch event selectNewBrand that tells the parent to change filter brand
     */
    selectNewBrand(idBrand){
      this.$emit("selectNewBrand",idBrand)
    },
    /**method to change order asc or desc
     **emit to parent new event changeOrder
     ** @isAsc boolean value for order
     */
    changeOrder(isAsc){
      this.$emit("changeOrder",isAsc)
    },
    /**
     * method to change search filter,called from child
     ** emito to parent new event changeSearch
     ** @search new search value 
     */
    changeSearch(search){
        this.$emit("changeSearch",search)
    }
    
  },
  components: {
    TableHeader,
    TableRow,
  },
  mounted(){
  }

};
</script>

<style>
</style>