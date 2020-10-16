<template>
    <section class="sound">
        <audio id="audio" src="~/assets/IncorrectAnswer.mp3"></audio>
        <b-button @click="clickMe">Play</b-button>
    </section>
</template>

<script>

export default 
{
    name: "IncorrectSound",

    methods: {
        clickMe() 
        {
            audio.play();
        }
    }
}
</script>

<style>

</style>