<template>
    <div class="row">
        <div class="row">
            <div class="col-2"></div>
            <div class="col-8 h2 my-5"> Brand Update</div>
            <div class="col-2">
                <div class="alert  alert-danger alert1" role="alert" v-if="isAlertShown">
                    {{alertMessage}}
                </div>
                <div class="alert  alert-danger alert2" role="alert" v-if="isAlert2Shown">
                    {{alert2Message}}
                    <i class="bi bi-bookmark-x position-absolute top-0 end-0" @click="hideAlert2"></i>
                </div>
            </div>
            </div>
        <div class="row">
            <p v-for="(error,index) in errors" :key="index">{{error}}</p>
        </div>
        <div class="row">
            <div class="col-2"></div>
            <div class="col-8">


            <form @submit.prevent="sendData" >
                <div class="form-group row mt-2">
                <label >Name</label>
                <input type="text" class="form-control" v-model="form.brandName" >
                </div>
                <div class="form-group row mt-2">
                    <label > Description</label>
                    <input type="text" class="form-control" v-model="form.description" >
                </div>

                <button class="btn btn-outline-primary mt-2 row" >Invio</button>






            </form>

        </div>
        <div class="col-2">
            
        </div>
        



        </div>
        
        






    </div>
</template>


<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");

export default ({
    data(){
        return {
            id:0,
            form:{
                id:0,
                brandName:"",
                description:""
            },
            isLoading:true,
            errors:[],
            alertMessage:"OPS!!! A error Happened yoyo!!",
            isAlertShown:false,
            alert2Message:"OPS!!! Big Error DUDE",
            isAlert2Shown:false
        }
    },
    methods:{
        async load(){
            this.isLoading=true
            this.id=this.$route.params.id
            await BrandRepository.getDataForEdit(this.id)
            .then(data=>{
                data=data.data
                this.form.brandName=data.brandName
                this.form.description=data.description
                this.form.id=data.id
                this.isLoading=false
            })
            .catch(err=>{
                this.alert2Message=err
                this.isAlert2Shown=true
            });
        },
        async sendData(){
            if(this.checkForm){
                await BrandRepository.editBrand(this.form)
                    .then(()=>this.$router.push("/brands/"+this.id))
                    .catch(err=>{
                        this.showAlert(err)
                    });
            }
            
        },
        checkForm(){
            if(this.form.brandName=="")
                this.errors.push("Brand name not valid")
            if(this.form.description=="")
            this.errors.push("Brand description not valid")
            if(this.errors.length>0)
                return false

            return true
        },
        showAlert(err){
            this.alertMessage=err
            this.isAlertShown=true
            this.scrollToTop()
            setTimeout(() => this.hideAlert(2000), 6000)
        },
        hideAlert(){
            this.isAlertShown=false
        },
        scrollToTop() {
            window.scrollTo(0,0);
        },
        showAlert2(err){
            this.alert2Message=err
            this.showAlert2=true
        },
        hideAlert2(){
            this.isAlert2Shown=false
        },
    },
    async created(){
        await this.load()
        this.$emit("setActiveLink",2)
    },
    beforeCreate(){
        
    }
})
</script>
