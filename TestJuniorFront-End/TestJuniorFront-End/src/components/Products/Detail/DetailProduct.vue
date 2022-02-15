<template>
    <div v-if="!isLoadingProduct" >
        
        <div class="row ">

            <div class="col-1">
                
            </div>
            <div class="col-8">
                
                <div class="row">
                    <p class="h3">
                        {{product.name}} by {{product.brandName}}
                    </p>
                </div>

                <div v-if="product.productsCategory.length>0">
                    <div class="row mt-4">
                    <p>Categorie associate al prodotto</p>
                </div>
                <div class="row">
                    <categories-list :categories="product.productsCategory"></categories-list>
                </div>
                </div>
                <div v-else class="mt-5">
                    <b> Non ci sono Categorie associate al prodotto</b>
                </div>
                

                

                <div class="row mt-3">
                    <p><b>Leads per questo prodotto</b></p>
                </div>
                <div class="row">
                    <p>{{message}}</p>
                </div>
                

                
                <div class="row mt-4 " v-if="product.infoRequestProducts.length!=0">
                    <div v-if="!viewInfoRequests" >
                        <button class="btn btn-outline-primary mb-5" 
                                @click="viewInfoRequests=!viewInfoRequests">Vedi tutte le richieste informazioni in questa pagina</button>
                    </div>
                    
                </div>
                <div class="row position-relative mt-3 " v-if="viewInfoRequests">
                    
                    <div class="row bg-g box">
                        <div class="col-1">
                            Id
                        </div>
                        <div class="col-3">Name
                        </div>
                        <div class="col-3">
                            LastName
                        </div>
                        <div class="col-3">
                            Date Last Reply
                        </div>
                        <div class="col-2">
                            Reply Number
                        </div>
                    </div>

                    <button class="close-btn btn btn-danger  position-absolute top-0 end-0" 
                                @click="viewInfoRequests=!viewInfoRequests"><i class="bi bi-x-octagon d-flex justify-content-center"></i></button>
                    
                    <div class="row " 
                        v-for="(ir,index) in product.infoRequestProducts"
                        :key="ir.id"
                        :class="[index%2==0?'bg-g':'']">
                        <div class="col-1">
                            {{ir.id}}
                        </div>
                        <div class="col-3">
                            {{ir.name}}
                        </div>
                        <div class="col-3">
                            {{ir.lastName}}
                        </div>
                        <div class="col-3">
                            {{dataformated(ir.dateLastReply)}}
                        </div>
                        <div class="col-2">
                            {{ir.replyNumber}}
                        </div>
                    </div>
                    <div class="row mb-5">

                    </div>
                </div>
            </div>
            <div class="col-2">
                <div class="row mt-4 visLeadsBtn">
                        <button class="btn btn-outline-primary mb-5 " 
                        @click="$router.push({ name: 'InfoRequestList', params: { productId: product.id}})" 
                        >Vedi tutte le richieste informazioni</button><br>
                </div>
            </div>
        </div>
        <div class="col-1"></div>




    </div>
    <div v-else>
        <my-detail-page-skeleton message="Product Detail"></my-detail-page-skeleton>
    </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
import CategoriesList from "../../Generic/CategoriesList.vue"
import MyDetailPageSkeleton from "../../Generic/MyDetailPageSkeleton.vue"
export default ({
    data(){
        return {
            /**product id taken from toute */
            idPorduct:this.$route.params.id,
            /**data coming from api */
            product:null,
            isLoadingProduct:true,
            /**message for the page completed with a method on component cretion */
            message:"",
            /**bool to see or not info requests */
            viewInfoRequests:false
        }
    },
    methods:{
        /**load the neccessary data for the page from api */
        async load(){
            this.isLoadingProduct=true;
            const { data } = await ProductRepository.getProduct(
            this.idPorduct
            );
            this.product=data
            this.isLoadingProduct=false;

        },
        /**makes the message needed in the page */
        makeMessage(){
            let guestIRC=this.product.countGuestInfoRequests
            let userIRC=this.product.countUserInfoRequests
            let totalIRC=guestIRC+userIRC
            if(totalIRC>0){
                if(guestIRC==0){
                    this.message=totalIRC+" richieste informazioni per questo prodotto, ricevute tutte da utenti registrati"
                }else{
                    if(userIRC==0){
                    this.message=totalIRC+" richieste informazioni per questo prodotto, ricevute tutte da utenti guest"

                    }else{
                        this.message=`${totalIRC} richieste informazioni ricevute per questo prodotto fra cui ${guestIRC} da utenti guest e ${userIRC} da utenti registrati`;
                    }
                }
                
            }
            else{
                this.message="Non ci sono info request per questo prodotto"
            }
        },
        /**removes the time from datetime */
        dataformated(data){
            let datanew=data.split("T")
            return datanew[0]
        }
    },
    async created(){
        await this.load()
        this.makeMessage()
        this.$emit("setActiveLink",1)
    },
    components:{
        CategoriesList,
        MyDetailPageSkeleton
    }
})
</script>

<style  scoped>
.close-btn{
    max-width: 30px;
    max-height: 30px;
}
.bg-g{
    background-color: rgb(218, 218, 218);
}
.box{
    border-bottom:2px solid black;
}
.visLeadsBtn{
    
  position: sticky;
  top: 60px;
    max-width: 98%;
    float: right;
  }
</style>


