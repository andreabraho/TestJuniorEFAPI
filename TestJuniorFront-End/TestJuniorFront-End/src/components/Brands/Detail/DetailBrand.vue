<template>
    <div v-if="!isLoadingBrand" class="mt-5 row">


            <div class="col-2"></div>
            
            <div class="col-8 ">

            <div class="row h2 ">
                {{brand.name}}
            </div>

            <div class="row mt-5">
                <p><b>Categorie associate ai prodotti di {{brand.name}}</b></p>
            </div>

            <div class="row" v-if="brand.associatedCategory.length>0">
                <categories-list :categories="brand.associatedCategory"></categories-list>
            </div>
            <div v-else>
                <p class="ms-5 h-6">Non ci sono categorie associate ai prodotti del brand</p>
            </div>

            <div class="row mt-5">
                <p><b>Richieste Informazioni per Prodotto</b></p>
            </div>

            <div class="row">
                <p>{{message}}</p>
            </div>

            <div class="row prod-list p-2 text-light bg-g"
                v-if="productsPage.length>0">
                <div class="col-1">
                    Id
                </div>
                <div class="col-7">
                    Product Name
                </div>
                <div class="col-4">
                    Num Richiese Informazioni
                </div>
            </div>
            <div class="row prod-list p-2" 
                    v-for="(item,index) in productsPage" 
                    :key="item.id"
                    :class="[index%2==0?'bg-g':'']">
                <div class="col-1 ">
                    {{item.id}}
                </div>
                <div class="col-7">
                    {{item.name}}
                </div>
                <div class="col-4">
                    {{item.countInfoRequest}}
                </div>
            </div>

            <div class="row" v-if="productsPage.length>0">
                <page-buttons 
                
                    class="d-flex justify-content-center"
                    :page="page"
                    :maxPages="Math.ceil(brand.products.length/pageSize)"
                    @changePage="changePage"
                    ></page-buttons>
            </div>



            </div>

            <div class="col-2"></div>

    </div>
    <div v-else>
        <my-detail-page-skeleton message="Brand detail"></my-detail-page-skeleton>
    </div>
</template>


<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");
import CategoriesList from "../../Generic/CategoriesList.vue"
import PageButtons from "../../Generic/PageButtons.vue"
import MyDetailPageSkeleton from "../../Generic/MyDetailPageSkeleton.vue"
export default ({
    data(){
        return {
            /**id of the brand taken from root used to load the brand */
            idBrand:this.$route.params.id,
            /**data coming from api */
            brand:null,
            isLoadingBrand:true,
            /**message shown in page filled with a method */
            message:"",
            page:1,
            pageSize:5,
        }
    },
    computed:{
        /**products for the current page */
        productsPage() {
            let l=this.brand.products.length;
            let start=(this.page-1)*this.pageSize;
            //let end=start+this.pageSize>l?start+this.pageSize:l;
            let end=0
            if(start+this.pageSize<l)
                end=start+this.pageSize
            else
                end=l

            let array=this.brand.products.slice(start,end);
            return array
        }
    },
    methods:{
        /**load the neccessary data for the page from api */
        async load(){
            this.isLoadingBrand=true;
            const { data } = await BrandRepository.getBrand(
            this.idBrand
            );
            this.brand=data
            this.isLoadingBrand=false;
        },
        /**makes the message to show in page based on data */
        makeMessage(){
            if(this.brand.countRequestFromBrandProducts>0){
                this.message=this.brand.countRequestFromBrandProducts+" richieste informazioni raccolte su un totale di "+this.brand.products.length+ " prodotti";
            }else{
                this.message="Non ci sono rieste informazioni per i prodotti"
            }
        },
        /**called from child component changes the current page */
        changePage(num){
            this.page=num
        }
    },
    async created(){
        await this.load()
        this.makeMessage()
        this.$emit("setActiveLink",2)

    },
    components:{
        CategoriesList,
        PageButtons,
        MyDetailPageSkeleton
    }
})
</script>

<style scoped>
    .prod-list{
        border:2px solid rgb(193, 211, 167);
    }
    .bg-g{
        background-color: rgb(204, 230, 166);
    }
</style>
