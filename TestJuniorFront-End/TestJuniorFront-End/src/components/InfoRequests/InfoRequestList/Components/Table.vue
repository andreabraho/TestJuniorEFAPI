<template>
  <div>

      <table-header :brands="brands" 
                    @selectNewBrand="selectNewBrand"
                    @changeOrder="changeOrder"
                    @changeSearch="changeSearch"
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
    changeOrder(isAsc){
      this.$emit("changeOrder",isAsc)
    },
    changeSearch(search){
        this.$emit("changeSearch",search)
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