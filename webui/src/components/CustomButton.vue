<template>
  <q-btn v-if="btn.type == 0" rounded :label="btn.label" :style="'background-color:' + btn.color" :icon="btn.icon"
    @click="btnExecute(btn)" no-caps />
  <q-toggle v-else-if="btn.type == 1" :label="btn.label" color="blue" :unchecked-icon="btn.icon" checked-icon="done"
    v-model="checked" @update:model-value="val => toggleExecute(val, btn)"></q-toggle>

  <div v-else-if="btn.type == 2">
    <q-badge color="secondary">
      <q-icon v-if="btn.icon" :name="btn.icon" />
      {{ btn.label + ': ' + context.value }}
    </q-badge>
    <q-slider :min="btn.min" :max="btn.max" v-model="context.value" :step="0"
      @update:model-value="val => sliderExecute(val, btn)" :style="'background-color:' + btn.color"></q-slider>
  </div>
  <q-btn v-else-if="btn.type == 3" rounded :label="btn.label" :style="'background-color:' + btn.color" :icon="btn.icon"
    @click="showDialog = true" no-caps>
    <q-dialog v-model="showDialog" persistent>
      <q-card style="min-width: 350px">
        <q-card-section>
          <div class="text-h6">{{ btn.label }}</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <q-input dense v-model="context.textValue" autofocus @keyup.enter="showDialog = false; btnExecute(btn)" />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat :label="$t('cancel')" v-close-popup />
          <q-btn flat :label="$t('confirm')" v-close-popup @click="btnExecute(btn)" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-btn>
</template>

<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';

export default defineComponent({
  name: 'CustomButton',
  data() {
    return {
      checked: false,
      context: {
        value: 1e-11,
        textValue: ''
      },
      busy: false,
      lockValue: null,
      showDialog: false
    };
  },
  props: {
    btn: {
    }
  },
  mounted() {
    this.context.osc = this.oscControl;
    this.context.chat = this.chat;
  },
  methods: {
    chat(message) {
      this.oscControl("/chatbox/input", [message, true, true]);
    },
    oscControl(path, data) {
      if (this.busy) {
        this.lockValue = { path: path, data: data };
        return;
      }
      this.busy = true;

      api.request({
        method: "post",
        url: "/api/osc?path=" + path,
        data: data
      }).finally(() => {
        this.busy = false;
        if (this.lockValue) {
          this.oscControl(this.lockValue.path, this.lockValue.data);
          this.lockValue = null;
        }
      });
    },
    btnExecute(btn) {
      this.execute(btn.action);
    },
    toggleExecute(value, btn) {
      if (value)
        this.execute(btn.action);
      else
        this.execute(btn.deactiveAction);
    },
    sliderExecute(value, btn) {
      this.context.value = value;
      this.context.value += 1e-11;
      this.execute(btn.action);
    },
    execute(action) {
      const func = new Function(action);
      func.call(this.context);
    }
  }
})
</script>
