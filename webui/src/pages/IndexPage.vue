<template>
  <q-page class="flex justify-center">
    <q-card style="max-width: 500px; width: 100%;">
      <q-card-section>
        <q-input :label="$t('chat')" maxlength="144" autogrow v-model="message">
          <template v-slot:after>
            <q-btn :label="$t('send')" @click="chat">
            </q-btn>
            <!-- <q-btn-dropdown split :label="$t('send')" @click="chat">
              <q-list>
                <q-item clickable v-close-popup @click="duration = 30" :active="duration == 30">30s</q-item>
                <q-item clickable v-close-popup @click="duration = 60" :active="duration == 60">60s</q-item>
              </q-list>
            </q-btn-dropdown> -->
          </template>
        </q-input>
        <q-btn flat color="red" :label="$t('clear')" @click="message = ''" />
        <q-btn :label="$t('myTime')" @click="addDate" />
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-badge color="secondary"> {{ turning }} </q-badge>
        <q-slider v-model="turning" :min="-1.0" :max="1.0" :step="0"
          @update:model-value="val => oscControl('/input/LookHorizontal', [val + 1e-11])" />
        <q-btn :label="$t('stop')" icon="stop_circle"
          @click="oscControl('/input/LookHorizontal', [1e-11]); turning = 0"></q-btn>
        <q-separator />
        <q-badge color="secondary"> {{ vertical }} </q-badge>
        <q-slider v-model="vertical" :min="-1.0" :max="1.0" :step="0"
          @update:model-value="val => oscControl('/input/Vertical', [val + 1e-11])" />
        <q-btn :label="$t('stop')" icon="stop_circle"
          @click="oscControl('/input/Vertical', [1e-11]); vertical = 0"></q-btn>
      </q-card-section>
      <q-separator />
      <q-card-section>
        <CustomButton v-for="btn in btns" :key="btn.id" :btn="btn" />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';
import CustomButton from '../components/CustomButton.vue'

export default defineComponent({
  name: 'IndexPage',
  data() {
    return {
      message: "",
      // duration: 30,
      avatar: {},
      turning: 0,
      vertical: 0,
      category: 0,
      btns: []
    }
  },
  components: {
    CustomButton
  },
  created() {
    const cat = this.$q.sessionStorage.getItem('btn_cat');
    if (cat)
      this.category = cat;
  },
  mounted() {
    this.getBtns();
  },
  methods: {
    getBtns() {
      api.request({
        url: "/api/button/search?category=" + this.category
      }).then((resp) => {
        this.btns = resp.data;
      }).catch(() => {
        setTimeout(() => {
          this.getBtns();
        }, 5000);
      });
    },
    chat() {
      this.oscControl("/chatbox/input", this.message);
      this.message = "";
    },
    addDate() {
      this.message = this.message + new Date(Date.now()).toTimeString();
    },
    oscControl(path, data) {
      api.request({
        method: "post",
        url: "/api/osc?path=" + path,
        data: data
      });
    }
  }
})
</script>
