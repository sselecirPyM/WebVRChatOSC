<template>
  <q-page class="flex justify-center bg-grey">
    <q-card flat style="max-width: 500px; width: 100%;">
      <q-card-section>
        <q-select v-model="locale" :options="localeOptions" label="Language" dense borderless emit-value map-options
          options-dense style="min-width: 150px" @update:model-value="languageChange">
          <template v-slot:prepend>
            <q-icon name="language" />
          </template>
        </q-select>
      </q-card-section>
      <q-card-section align="right">
        <q-form>
          <q-input :label="$t('hostAddress')" maxlength="128" v-model="host" />
          <q-input :label="$t('recvport')" v-model="recvport" type="number" />
          <q-input :label="$t('sendport')" v-model="sendport" type="number" />
          <q-btn flat color="blue" type="submit" :label="$t('change')" @click="change" />
        </q-form>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';
import { useI18n } from 'vue-i18n'

export default defineComponent({
  name: 'SettingsPage',
  setup() {
    const { locale } = useI18n({ useScope: 'global' })

    return {
      locale,
      localeOptions: [
        { value: 'en-US', label: 'English' },
        { value: 'zh-CN', label: '中文' }
      ]
    }
  },
  data() {
    return {
      host: "",
      recvport: 0,
      sendport: 0
    }
  },
  mounted() {
    api.request({
      url: "/api/management/address"
    }).then((resp) => {
      this.host = resp.data.host;
      this.recvport = resp.data.recv_port;
      this.sendport = resp.data.send_port;
    });
  },
  methods: {
    languageChange(val) {
      console.log(val);
      this.$q.localStorage.set("language", val);
    },
    change() {
      api.request({
        method: "post",
        url: "/api/management/address",
        data: {
          host: this.host,
          send_port: this.sendport,
          recv_port: this.recvport
        }
      });
    }
  }
})
</script>
