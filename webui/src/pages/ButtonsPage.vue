<template>
  <q-page class="flex justify-center bg-grey">
    <q-card flat style="max-width: 500px; width: 100%;">
      <q-expansion-item expand-separator :label="$t('EditButtons')">
        <q-card-section>
          <AddButton :category="category" @submit="getBtns" />
        </q-card-section>
        <q-separator />
        <q-toggle v-model="showDelete" :label="$t('delete')" checked-icon="delete" color="red"
          @update:model-value="beginDelete"></q-toggle>
        <q-btn dense :label="$t('exportJson')" @click="exportJson" />
        <div style="width: 100px; height: auto; display: inline-block;">
          <q-file :label="$t('importJson')" dense outlined v-model="file" @update:model-value="uploadJson" />
        </div>
      </q-expansion-item>
      <q-card-section>
        <q-tabs dense v-model="category">
          <q-tab v-for="i in categoryCount" :key="i - 1" :label="i - 1" :name="i - 1" />
        </q-tabs>
        <q-tab-panels v-model="category" animated swipeable>
          <q-tab-panel v-for="(btns, index) in btnGroups" :key="index" :name="index">
            <CustomButton v-show="!showDelete" v-for="btn in btns" :key="btn.id" :btn="btn" />
            <template v-if="showDelete">
              <!-- <q-btn v-for="btn in btns" :key="btn.id" color="red" :label="btn.label" icon="delete"
                        @click="deleteBtn(btn)" no-caps /> -->
              <q-toggle v-for="btn in btns" :key="btn.id" v-model="btn.delete" :label="btn.label" color="red"
                checked-icon="delete" :unchecked-icon="btn.icon"></q-toggle>
            </template>
          </q-tab-panel>
        </q-tab-panels>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';
import CustomButton from '../components/CustomButton.vue'
import AddButton from '../components/AddButton.vue'

export default defineComponent({
  name: 'ButtonsPage',
  data() {
    return {
      category: 0,
      vbtns: [],
      btnGroups: [],
      showDelete: false,
      categoryCount: 10,
      file: null
    }
  },
  components: {
    CustomButton,
    AddButton
  },
  watch: {
    category(newCategory, oldCtegory) {
      this.$q.sessionStorage.set('btn_cat', newCategory);
    }
  },
  created() {
    const cat = this.$q.sessionStorage.getItem('btn_cat');
    if (cat)
      this.category = cat;
  },
  mounted() {
    for (let i = 0; i < this.categoryCount; i++) {
      this.btnGroups.push([]);
    }
    this.getBtns();
  },
  methods: {
    getBtns() {
      api.request({
        url: "/api/button/search"
      }).then((resp) => {
        this.vbtns = resp.data;
        this.btnGroups = [];
        for (let i = 0; i < this.categoryCount; i++) {
          this.btnGroups.push([]);
        }
        for (let i = 0; i < this.vbtns.length; i++) {
          const vbtn = this.vbtns[i];
          const category = vbtn.category;
          if (category >= 0 && category < this.categoryCount) {
            this.btnGroups[category].push(vbtn);
          }
        }

      }).catch(() => {
        setTimeout(() => {
          this.getBtns();
        }, 5000);
      });
    },
    beginDelete(val) {
      const btns = this.vbtns;
      if (val) {
        for (let i = 0; i < btns.length; i++) {
          btns[i].delete = false;
        }
      } else {
        const delbtns = [];
        for (let i = 0; i < btns.length; i++) {
          if (btns[i].delete) {
            delbtns.push(this.vbtns[i].id);
          }
        }
        if (delbtns.length > 0) {
          api.request({
            method: "post",
            url: "/api/button/delete-many",
            data: delbtns
          }).then(this.getBtns);
        }
      }
    },
    // deleteBtn(btn) {
    //     api.request({
    //         method: "post",
    //         url: "/api/button/delete",
    //         data: btn.id
    //     }).then(this.getBtns);
    // },
    exportJson() {
      const jsonString = JSON.stringify(this.btnGroups[this.category], (key, value) => {
        if (key === "id" || key === "category") {
          return undefined;
        }
        return value;
      }, 2);
      const blob = new Blob([jsonString], {
        type: "application/json",
      });
      const url = URL.createObjectURL(blob);
      let a = document.createElement('a');
      a.href = url;
      a.download = "export.json";
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    },
    uploadJson(file) {
      console.log(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.importJson(reader.result);
      }
      reader.readAsText(file);
    },
    importJson(jsonString) {
      const data = JSON.parse(jsonString)
      for (let i = 0; i < data.length; i++) {
        data[i].category = this.category;
      }
      api.request({
        method: "post",
        url: "/api/button/add-many",
        data: data
      }).then(this.getBtns);
    }
  }
})
</script>
  