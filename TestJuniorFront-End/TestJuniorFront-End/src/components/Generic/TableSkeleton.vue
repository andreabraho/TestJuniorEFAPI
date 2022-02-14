<template>
  
    <div>

        <div class="header row">

            <div class="col header-col" v-for="index in col" :key="index">

            </div>


        </div>

        <div class="body">

            <div class="row" v-for="index in rows" :key="index">

                <div class="col body-col" v-for="index in col" :key="index">
                    a
                </div>

            </div>




        </div>










    </div>











</template>

<script>
export default {
    props:{
        rows:{
            type:Number,
            default:10
        },
        col:{
            type:Number,
            default:4
        }
    }
}
</script>

<style>
.body-col {
	width: 90%;
	padding-top: 100%;
	height: 16px;
	animation: pulse-bg 1s infinite;
    border:1px solid black
}
.header-col {
	width: 60%;
	height: 16px;
	display: block;
	animation: pulse-bg 1s infinite;
    border: 1px solid black;
}
.profile-info h3 {
	width: 250px;
	height: 24px;
	animation: pulse-bg 1s infinite;
}
.profile-info p {
	width: 80%;
	height: 16px;
	animation: pulse-bg 1s infinite;
}
</style>