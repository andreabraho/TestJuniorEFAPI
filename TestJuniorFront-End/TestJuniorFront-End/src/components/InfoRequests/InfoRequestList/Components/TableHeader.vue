<template>
  <div>
    <div class="row ">
      <div class="col-1">Id</div>
      <div class="col-2">NomeBrand</div>

      <div class="col-2">NomeProdotto</div>
      <div class="col-2">User</div>
      <div class="col-3">RequestText</div>

      <div class="col-2 mw-10 position-relative orderbox mh-36 click-box disable-select"
        v-debounce:150ms="changeOrder" debounce-events="click"
        >Data
          
        <i class="bi bi-caret-up-fill position-absolute top-0 end-0 i-g"
        :class="[isAsc?'i-b':'i-g']"></i>
        
        <i class="bi bi-caret-down-fill position-absolute bottom-0 end-0 i-g"
        :class="[!isAsc?'i-b':'i-g']"></i>
      
      </div>

    </div>

    <div class="row  bg-grey t-header">
      <div class="col-1 mw-10"></div>
        
      <div class="col-2 mw-10 mb-2 mt-2" >

        <select class="form-select" 
                @change="changeBrand" 
                v-model="selectedBrand">
          <option default value="0">Tutti i Brand</option>

          <option   v-for="brand in brands" 
                    :key="brand.id" 
                    :value="brand.id">{{brand.name}}</option>
          
        </select>
      </div>
      <div class="col-2 mw-40 mt-2 ">
          <div class="input-group">
              <input type="text" 
                    class="form-control"
                    v-model="search" >
            <button class="btn btn-primary"
                    @click="changeSearch">
              <i class="bi bi-search"></i>
          </button>
          </div>
          
      </div>
      <div class="col-2 mw-30 mt-2">
          
      </div>
      <div class="col-3 mw-10"></div>
      <div class="col-2 mw-10"></div>
    </div>
  </div>
</template>



<script>
import debounce from 'lodash/debounce'
export default {
  name: "TableHeader",
  props:{
      brands:[],
      searchProp:String,
      selectedBrandProp:Number
  },
  data(){
      return{
          /** id brand to filter selected */
          selectedBrand:0,
          search:null,
          isAsc:false
      }
  },
  methods:{
      /** event that launches event selectnewBrand to tell the parent to change the brand filter
      ** payload the selected brand */
      changeBrand: debounce(function(){
          this.$emit("selectNewBrand",parseInt(this.selectedBrand))
      },150),
      /**method to change order asc or desc
       ** emit event changeOrder for to tell parent to change the order
        */
      changeOrder:debounce(function(){
          this.isAsc=!this.isAsc
          this.$emit("changeOrder",this.isAsc)
      },150),
      /**
       * eveng to change search filter
       ** emit changeSearch event to tell the parent to update the list with new search
       */
      changeSearch:debounce(function(){
          this.$emit("changeSearch",this.search)

      },150),
      
  },
  created(){
    this.selectedBrand=this.selectedBrandProp
    this.search=this.searchProp
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
    .click-box:hover{
        border-top:1px solid gainsboro;
        border-left:1px solid gainsboro;
        
    }
    .disable-select{
        user-select:none;
    }

</style>