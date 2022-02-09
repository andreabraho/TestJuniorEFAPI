<template>
  <div class="row">
      <div class="col-1"></div>
      <div class="col-7">
          <div class="d-flex justify-content-start mt-3"> 
            <span class="h2 ">Create New Product</span>
          </div>

          <form @submit.prevent="sendData" class="mt-5 mb-5">
            <div class="form-group">
                <label >Name</label>
                <input type="text" class="form-control" v-model="form.product.name">
            </div>
            <div class="form-group">
                <label >Short Description</label>
                <input type="text" class="form-control" v-model="form.product.shortDescription">
            </div>
            
            <div class="form-group">
                <label >Description</label>
                <textarea class="form-control"  rows="7"
                        v-model="form.product.description"></textarea>
            </div>
            <div class="form-group">
                <label >Price</label>
                <input type="number" class="form-control" v-model="form.product.price">
            </div>
            <div>
                <label for="">Brand</label>
                <select class="form-select" v-model="form.product.brandId">
                <option selected>Select Brand</option>
                <option v-for="item in dataForCreate.brands" 
                        :key="item.id"
                        :value="item.id">{{item.name}}</option>
                </select>
            </div>
            <div>
                <label for="">Categorie</label>
                <select class="form-select" 
                        multiple  
                        v-model="form.categoriesIds">
                <option selected>Select Categories</option>
                <option 
                v-for="item in dataForCreate.categories"
                :key="item.id"
                :value="item.id">{{item.name}}</option>
                
                </select>
            </div>
            <div class="d-flex justify-content-end">
            <button class="btn btn-outline-primary mt-2 " >Invio</button>
            </div>
        </form>

      </div>
      <div class="col-4 "></div>
    
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
export default {
    data(){
        return{
            form:{
                product:{
                    name:"",
                    shortDescription:"",
                    description:"",
                    price:0,
                    brandId:0
                },
                categoriesIds:[],
                
            },
            dataForCreate:null,
            isLoading:false
        }
    },
    methods:{
        async load(){
            this.isLoading=true
            const { data } = await ProductRepository.getDataForCreate();
            console.log(data)
            this.dataForCreate=data
            this.isLoading=false
        },
        async sendData(){
            const { data } =await ProductRepository.createProduct(this.form);
            console.log(data)
            this.$router.push("/products/"+data)
        }
    },
    async created(){
        await this.load()
    }
}
</script>

<style>

</style>