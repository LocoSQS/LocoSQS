import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListQueuesComponent } from './list-queues.component';

describe('ListQueuesComponent', () => {
  let component: ListQueuesComponent;
  let fixture: ComponentFixture<ListQueuesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ListQueuesComponent]
    });
    fixture = TestBed.createComponent(ListQueuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
