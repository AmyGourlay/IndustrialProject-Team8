<template>
    <section class="sound">
        <audio id="audio" src="~/assets/Quizz.mp3" v-on:canplay="start" loop></audio>
        <img class="img" src="~/assets/volume_on.png" @click="toggle" alt="Toggle Sound" height="80px" width="80px" align="right">
    </section>
</template>

<script>

export default 
{
    name: "GameMusic",

    methods: {
        start()
        {
            var a = document.getElementById("audio")
            a.volume = 0.2
            a.play()
        },

        toggle() 
        {
            if(!audio.paused)
            {
                audio.pause();
            }
            else
            {
                audio.play();
            }
                
        }
    }
}
</script>

<style>

</style>