
<template>
  <div class="row" v-if="!isLoading">
      <div class="col-1"></div>
      <div class="col-7">
          <div class="d-flex justify-content-start mt-3 row"> 
            <span class="h2 ">{{mainMessage}}</span>
          </div>
          
          <div v-if="errors.length>0">
              <p v-for="(error,index) in errors"
                :key="index">{{error}}</p>
          </div>
            

          <form @submit.prevent="sendData" class="mt-5 mb-5" >
            <div class="row mt-2">
                <label >Name</label>
                <input type="text" 
                        class="form-control" 
                        v-model="form.product.name"
                        @blur="validateName"
                        :class="[errors.name!=null?'error-input':'']"
                        >
                <div class="text-danger">{{errors.name}}</div>
            </div>
            <div class=" row mt-2">
                <label >Short Description</label>
                <input type="text" class="form-control" 
                        v-model="form.product.shortDescription"
                        @blur="validateShortDescription"
                        :class="[errors.shortDescription!=null?'error-input':'']"
                        >
                <div class="text-danger">{{errors.shortDescription}}</div>
            </div>
            
            <div class="row mt-2">
                <label >Description</label>
                <textarea class="form-control"  
                        rows="7"
                        v-model="form.product.description"></textarea>
            </div>
            <div class=" row mt-2">
              
                
            </div>
            
            <div  class="row mt-2">
              <div class="col-6" v-if="id==0">
                <label for="">Brand</label>
                <select class="form-select" 
                        v-model="form.product.brandId" 
                        required
                        @blur="validateBrandId"
                        :class="[errors.brandId!=null?'error-input':'']"
                        >
                <option v-for="item in brandForSelect" 
                        :key="item.id"
                        :value="item.id"
                        >{{item.name}}</option>
                </select>
                <div class="text-danger">{{errors.brandId}}</div>
              </div>
              <div class="col-6">
                <label >Price</label>
                <input type="number" 
                      class="form-control" 
                      v-model="form.product.price" 
                      step=".01"
                      :class="[errors.price!=null?'error-input':'']"
                      @blur="validatePrice">
                <div class="text-danger">{{errors.price}}</div>
                </div>
                
            </div>

            <div class="row ">
              <div class="mt-5 checboxes"
                    :class="[errors.categories!=null?'error-input-catbox':'']"
                >
                  <div class="form-check form-check-inline col mt-3"
                  v-for="item in catForSelect"
                  :key="item.id">
                    <input class="form-check-input" type="checkbox" 
                            :value="item.id" 
                            v-model="form.categoriesIds"
                            @blur="validateCategories">
                    <label class="form-check-label" >{{item.name}}</label>
                  </div>
                  <div class="text-danger">{{errors.categories}}</div>
              </div>
              
            </div>

            

            <div class="d-flex justify-content-end row mt-2">
            <button class="btn btn-outline-primary mt-2 " >Invio</button>
            </div>
        </form>

      </div>
      <div class="col-4 ">

        <div class="alert alert-danger" role="alert">
          {{alertMessage}}
        </div>
      </div>
    
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
import $ from 'jquery'

export default {
  data() {
    return {
      form: {
        product: {
          id: 0,
          name: "",
          shortDescription: "",
          description: "",
          price: 0,
          brandId: 0,
        },
        categoriesIds: [],
      },
      isLoading: false,
      id: 0,
      catForSelect: null,
      brandForSelect: null,
      mainMessage: "",
      errors: {
        name:null,
        shortDescription:null,
        brandId:null,
        price:null,
        categories:null
      },
      alertMessage:"Ops!! There was an error!!"
    };
  },
  methods: {
    /**load all data neccessary at the creation of the component */
    async load() {
      this.isLoading = true;
      /**if id comes from route sets it to route id or set it to 0 */
      if (this.$route.params.id != undefined) this.id = this.$route.params.id;
      else this.id = 0;
      
      let data;
      if (this.id != 0) {
        data = await ProductRepository.getDataForUpdate(this.id);
      } else {
        data = await ProductRepository.getDataForCreate();
      }
      this.updateFormData(data.data);
    },
    /**sends data to api if data pass validation */
    async sendData() {
      if (this.formCheck()) {
        
        if (this.id != 0){
          await ProductRepository.editProduct(this.form)
            .then(data=>{
              this.id = data.data;
              this.$router.push("/products/" + this.id);
            })
            .catch(function (error) {
              this.showAlert(error);
            }).bind(this);
        }
        else {
          await ProductRepository.createProduct(this.form)
          .then(data=>{
            this.id = data.data;
            this.$router.push("/products/" + this.id);
          })
          .catch(err=>{
            this.showAlert(err);return null})
        }
      }
    },
    /**updates form data with data coming from api */
    updateFormData(data) {
      if (this.id != 0) {
        this.form.product.id = this.id;
        this.form.product.name = data.product.name;
        this.form.product.shortDescription = data.product.shortDescription;
        this.form.product.description = data.product.shortDescription;
        this.form.product.price = data.product.price;
        this.form.product.brandId = data.product.brandId;

        this.form.categoriesIds = data.categoriesAssociated;
        this.catForSelect = data.allCategories;
        this.brandForSelect = data.allBrands;
        this.mainMessage = "Update Product";
      } else {
        this.catForSelect = data.categories;
        this.brandForSelect = data.brands;
        this.mainMessage = "Create Product";
      }
      this.isLoading = false;
    },
    /**validation method for the form */
    formCheck() {
      this.resetErrors()
      this.checkErrors()

      if(!this.validateName() || !this.validateShortDescription() || !this.validateBrandId() || !this.validatePrice() || !this.validateCategories())
        return false
      return true
    },
    validateName(){
      this.errors.name=null
      if(this.form.product.name.length>0 && this.form.product.name.length<255)
        return true
      this.errors.name="Name must be between 1 and 255 characters"
      return false
    },
    validateShortDescription(){
      this.errors.shortDescription=null
      if(this.form.product.shortDescription.length>0 && this.form.product.shortDescription.length<255)
        return true
      this.errors.shortDescription="Short description must be between 1 and 255 characters"
      return false
    },
    validateBrandId(){
      this.errors.brandId=null
      if(this.form.product.brandId>0)
        return true
      this.errors.brandId="Select the brand"
      return false
    },
    validatePrice(){
      this.errors.price=null
      if(this.form.product.price>=0 && this.form.product.price<1e16)
        return true
      this.errors.price="Price must be between 0 and 1e16"
      return false
    },
    validateCategories(){
      this.errors.categories=null
      if(this.form.categoriesIds.length>0)
        return true
      this.errors.categories="Select at least one category"
      return true
    },
    checkErrors(){
      this.validateName()
      this.validateShortDescription()
      this.validateBrandId()
      this.validatePrice()
      this.validateCategories()
    },
    resetErrors(){
      this.errors={
        name:null,
        shortDescription:null,
        brandId:null,
        price:null,
        categories:null
      }
    },
    showAlert(err){
      this.alertMessage=err
      $('.alert').show()
      this.scrollToTop()
      setTimeout(() => this.hideAlert(2000), 6000)
    },
    hideAlert(num){
      $('.alert').slideUp(num)
    },
    scrollToTop() {
    window.scrollTo(0,0);
  }
  },
  async created() {

    await this.load();
    this.$emit("setActiveLink", 1);
     $('.alert').hide()

  },
  
};
</script>

<style scoped>
.myselect {
  height: 200px;
}
.checboxes{
  border: 1px solid rgb(206, 206, 206);
}
.error-input{
  border: 1px solid red !important;
}
.error-input-catbox{
  border-left: 1px solid red ;
}
</style>