import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueAttributesComponent } from './queue-attributes.component';

describe('QueueAttributesComponent', () => {
  let component: QueueAttributesComponent;
  let fixture: ComponentFixture<QueueAttributesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [QueueAttributesComponent]
    });
    fixture = TestBed.createComponent(QueueAttributesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
