<template>
    <div v-if="!isLoadingProduct" >
        
        <div class="row ">

            <div class="col-2">

            </div>
            <div class="col-8">
                
                <div class="row">
                    <p class="h3">
                        {{product.name}} by {{product.brandName}}
                    </p>
                </div>
                <div class="row mt-4">
                    <p>Categorie associate al prodotto</p>
                </div>
                <div class="row">
                    <categories-list :categories="product.productsCategory"></categories-list>
                </div>
                <div class="row mt-3">
                    <p><b>Leads per questo prodotto</b></p>
                </div>
                <div class="row">
                    <p>{{message}}</p>
                </div>
                <div class="row mt-4 " >
                    <div v-if="!viewInfoRequests" >
                        <button class="btn btn-outline-primary mb-5" 
                                @click="viewInfoRequests=!viewInfoRequests">Vedi tutte le richieste informazioni</button>
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
                            {{ir.datelastReply}}
                        </div>
                        <div class="col-2">
                            {{ir.replyNumber}}
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-2">

            </div>
        </div>




    </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
import CategoriesList from "../../Brands/Detail/Components/CategoriesList.vue"
export default ({
    data(){
        return {
            idPorduct:this.$route.params.id,
            product:null,
            isLoadingProduct:true,
            message:"",
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
        }
    },
    async created(){
        await this.load()
        this.makeMessage()
    },
    components:{
        CategoriesList
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
</style>


