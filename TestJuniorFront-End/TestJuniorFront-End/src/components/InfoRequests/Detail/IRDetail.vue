<template>
<div v-if="!isLoadingIR" class="row">
  <div class="col-2"></div>
  <div class="col-8">
    <div class="row mt-3">
      <p class="h4">Richiesta informazioni di {{infoRequest.name}} {{infoRequest.lastName}}<br>
        per il Prodotto {{infoRequest.productIRDetail.name}} di {{infoRequest.productIRDetail.brandName}}
      </p>
    </div>
    <div class="row mt-4">
      <p>
      <b>Dati del richiedente</b><br>
      {{infoRequest.name}} {{infoRequest.lastName}}<br>
      {{infoRequest.location}}
      
      </p>
    </div>

    <div class="row mt-3">
      
    </div>

  </div>
  <div class="col-2"></div>


</div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
export default {
  data() {
    return {
        idIR:this.$route.params.id,
        infoRequest:null,
        isLoadingIR:true
    };
  },
  methods:{
    /**load the neccessary data for the page from api */
    async load(){
            this.isLoadingIR=true;
            const { data } = await IRRepository.getInfoRequest(
            this.idIR
            );
            this.infoRequest=data
            this.isLoadingIR=false;

        }
  },
  created(){
    this.load()
  }
};
</script>