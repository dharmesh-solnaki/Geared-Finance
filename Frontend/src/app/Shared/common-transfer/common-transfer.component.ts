import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonTransfer } from 'src/app/Models/common-transfer.model';

@Component({
  selector: 'app-common-transfer',
  templateUrl: './common-transfer.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class CommonTransferComponent {
  isSearchingRequired: boolean = true;
  @Input() title1: string = '';
  @Input() title2: string = '';
  @Input() listInput: CommonTransfer[] = [];
  @Input() existedList: CommonTransfer[] = [];
  availableDivDisplayList: CommonTransfer[] = [];
  selectedDivDisplayList: CommonTransfer[] = [];
  originalSelectedDivDisplayList: CommonTransfer[] = [];

  tempAvailableList = new Map();
  tempSelectedList = new Map();
  @Output() selectedListEmitter = new EventEmitter<CommonTransfer[]>();

  ngOnInit(): void {
    this.availableDivDisplayList = this.listInput;
    this.sortDisplayList(this.availableDivDisplayList);
  }
  filterExistedList(): void {
    if (this.existedList) {
      this.availableDivDisplayList = this.listInput
        .map((category) => {
          const existedCategory = this.existedList.find(
            (existedCat) => existedCat.id === category.id
          );

          if (
            existedCategory &&
            category.subCategory &&
            category.subCategory.length > 0
          ) {
            // Filter out subcategories that exist in existedList
            const filteredSubCategories = category.subCategory.filter(
              (subCat) =>
                !existedCategory.subCategory.some(
                  (existedSub) => existedSub.id === subCat.id
                )
            );
            return new CommonTransfer(
              category.id,
              category.name,
              filteredSubCategories
            );
          }
          return category;
        })
        .filter(
          (category) => category.subCategory && category.subCategory.length > 0
        );

      // Update availableDivDisplayList after filtering

      // this.availableDivDisplayList = [...this.listInput];
      this.sortDisplayList(this.availableDivDisplayList);

      // Assign existedList to originalSelectedDivDisplayList
      this.originalSelectedDivDisplayList = [...this.existedList];
      this.selectedDivDisplayList = [...this.existedList];
      this.sortDisplayList(this.selectedDivDisplayList);
    }
  }

  sortDisplayList(list: CommonTransfer[]): void {
    list.sort((a, b) => a.name.localeCompare(b.name));
    list.forEach((item) => {
      item.subCategory.sort((a, b) => a.name.localeCompare(b.name));
    });
  }

  availableDivDisplayListSearch(event: Event): void {
    const searchValue = (event.target as HTMLInputElement).value;
    this.availableDivDisplayList = this.searchList(searchValue, this.listInput);
  }

  selectedDivDisplayListSearch(event: Event): void {
    const searchValue = (event.target as HTMLInputElement).value;
    this.selectedDivDisplayList = this.searchList(
      searchValue,
      this.originalSelectedDivDisplayList
    );
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
  handleListClick(item: CommonTransfer, type: number): void {
    const subCategoryIds = item.subCategory.map((subItem) => subItem.id);
    const targetMap =
      type === 0 ? this.tempAvailableList : this.tempSelectedList;
    if (targetMap.has(item.id)) {
      targetMap.delete(item.id);
    } else {
      targetMap.set(item.id, subCategoryIds);
    }
  }

  handleSublistClick(subCatId: number, catId: number, type: number): void {
    const targetMap =
      type === 0 ? this.tempAvailableList : this.tempSelectedList;
    const subCategoryIds = targetMap.get(catId) || [];
    const index = subCategoryIds.indexOf(subCatId);
    index > -1
      ? subCategoryIds.splice(index, 1)
      : subCategoryIds.push(subCatId);

    subCategoryIds.length
      ? targetMap.set(catId, subCategoryIds)
      : targetMap.delete(catId);
  }
  getHighlights(catId: number, subCatId: number, type: number): boolean {
    const targetMap =
      type === 0 ? this.tempAvailableList : this.tempSelectedList;
    return targetMap.get(catId)?.includes(subCatId) || false;
  }
  updateList(
    sourceList: CommonTransfer[],
    targetList: CommonTransfer[],
    tempList: Map<number, number[]>
  ): void {
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
          existingItem.subCategory.push(...matchingSubCategories);
        } else {
          targetList.push(
            new CommonTransfer(
              catId,
              matchingTransfer.name,
              matchingSubCategories
            )
          );
        }
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
    this.originalSelectedDivDisplayList = [...this.selectedDivDisplayList];
    this.selectedListEmitter.emit(this.selectedDivDisplayList);
  }

  addToSelectedList(): void {
    this.updateList(
      this.availableDivDisplayList,
      this.selectedDivDisplayList,
      this.tempAvailableList
    );
  }

  removeFromSelectedList(): void {
    this.updateList(
      this.selectedDivDisplayList,
      this.availableDivDisplayList,
      this.tempSelectedList
    );
  }
}
