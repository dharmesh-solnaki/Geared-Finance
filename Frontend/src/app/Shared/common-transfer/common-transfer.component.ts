import { Component, Input } from '@angular/core';
import { CommonTransfer } from 'src/app/Models/common-transfer.model';

@Component({
  selector: 'app-common-transfer',
  templateUrl: './common-transfer.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class CommonTransferComponent {
  isSearchingRequired: boolean = true;
  @Input() title: string = '';
  @Input() listInput: CommonTransfer[] = [];
  availableDivDisplayList: CommonTransfer[] = [];
  tempList: CommonTransfer[] = [];
  tempAvailableList = new Map();
  usedEquipmentIds: Set<number> = new Set();

  ngOnInit(): void {
    this.availableDivDisplayList = this.listInput;
    this.sortDisplayList(this.availableDivDisplayList);
  }
  ngOnChanges(): void {
    this.availableDivDisplayList = this.listInput;
    this.sortDisplayList(this.availableDivDisplayList);
  }

  sortDisplayList(list: CommonTransfer[]): void {
    list.sort((a, b) => a.name.localeCompare(b.name));
    list.forEach((item) => {
      item.subCategory.sort((a, b) => a.name.localeCompare(b.name));
    });
  }

  listSearchHandler(event: Event): void {
    const searchValue = (event.target as HTMLInputElement).value;
    this.availableDivDisplayList = this.searchList(searchValue, this.listInput);
  }

  searchList(
    searchValue: string,
    originalList: CommonTransfer[]
  ): CommonTransfer[] {
    const fieldValue = searchValue.toLowerCase().trim();
    const filteredList = fieldValue
      ? (originalList
          .map((item) => {
            const filteredSubCategories = item.subCategory.filter((subCat) =>
              subCat.name.toLowerCase().includes(fieldValue)
            );
            return filteredSubCategories.length > 0
              ? new CommonTransfer(item.id, item.name, filteredSubCategories)
              : null;
          })
          .filter((item) => item) as CommonTransfer[])
      : [...originalList];

    this.sortDisplayList(filteredList);
    return filteredList;
  }
  handleListClick(item: CommonTransfer): void {
    const subCategoryIds = item.subCategory.map((subItem) => subItem.id);
    const targetMap = this.tempAvailableList;
    if (targetMap.has(item.id)) {
      targetMap.delete(item.id);
      subCategoryIds.forEach((id) => this.usedEquipmentIds.delete(id));
    } else {
      targetMap.set(item.id, subCategoryIds);
      subCategoryIds.forEach((id) => this.usedEquipmentIds.add(id));
    }
  }

  handleSublistClick(subCatId: number, catId: number): void {
    const targetMap = this.tempAvailableList;
    const subCategoryIds = targetMap.get(catId) || [];
    const index = subCategoryIds.indexOf(subCatId);
    if (index > -1) {
      subCategoryIds.splice(index, 1);
      this.usedEquipmentIds.delete(subCatId);
    } else {
      subCategoryIds.push(subCatId);
      this.usedEquipmentIds.add(subCatId);
    }

    subCategoryIds.length
      ? targetMap.set(catId, subCategoryIds)
      : targetMap.delete(catId);
  }
  getHighlights(catId: number, subCatId: number): boolean {
    return this.tempAvailableList.get(catId)?.includes(subCatId) || false;
  }
  updateList(sourceList: CommonTransfer[], targetList: CommonTransfer[]): void {
    const tempList = this.tempAvailableList;

    const itemsToRemove: CommonTransfer[] = [];

    tempList.forEach((subCatIds, catId) => {
      const matchingTransfer = sourceList.find(
        (transfer) => transfer.id === catId
      );
      if (!matchingTransfer) return;

      const matchingSubCategories = matchingTransfer.subCategory.filter(
        (subCat) => subCatIds.includes(subCat.id)
      );

      if (matchingSubCategories.length > 0) {
        const existingItem = targetList.find((item) => item.id === catId);
        if (existingItem) {
          matchingSubCategories.forEach((subCat) => {
            // Check if the subcategory already exists
            const subCatExists = existingItem.subCategory.some(
              (existingSubCat) => existingSubCat.id === subCat.id
            );
            if (!subCatExists) {
              existingItem.subCategory.push(subCat);
            }
          });
        } else {
          targetList.push(
            new CommonTransfer(catId, matchingTransfer.name, [
              ...matchingSubCategories,
            ])
          );
        }

        // Remove subcategories that were moved from the source list
        matchingTransfer.subCategory = matchingTransfer.subCategory.filter(
          (subCat) => !subCatIds.includes(subCat.id)
        );
        if (matchingTransfer.subCategory.length === 0) {
          itemsToRemove.push(matchingTransfer);
        }
      }
    });

    itemsToRemove.forEach((item) => {
      const index = sourceList.indexOf(item);
      if (index > -1) {
        sourceList.splice(index, 1);
      }
    });

    this.sortDisplayList(targetList);
    this.sortDisplayList(sourceList);
    tempList.clear();
  }

  addToSelectedList(): void {
    this.updateList(this.availableDivDisplayList, this.tempList);
  }

  isTempListEmpty() {
    return this.tempAvailableList.size == 0;
  }
  clearTheTempList() {
    this.tempList = [];
  }
}
