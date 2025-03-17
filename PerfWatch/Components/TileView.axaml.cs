using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace PerfWatch.Components;

public class TileView : TemplatedControl
{
    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<TileView, int>(
        nameof(Rows));

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<TileView, int>(
        nameof(Columns));

    public static readonly StyledProperty<double> ItemWidthProperty = AvaloniaProperty.Register<TileView, double>(
        nameof(ItemWidth));

    public static readonly StyledProperty<double> ItemHeightProperty = AvaloniaProperty.Register<TileView, double>(
        nameof(ItemHeight));

    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<TileView, IEnumerable>(
            nameof(ItemsSource));

    public static readonly StyledProperty<TileItem?> ActivatedItemProperty =
        AvaloniaProperty.Register<TileView, TileItem?>(
            nameof(ActivatedItem));

    private WrapPanel? _itemsHostPanel;
    private ContentControl? _previewItemHost;
    private ScrollViewer? _itemsHost;

    public TileView()
    {
        Items.CollectionChanged += Items_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_itemsHostPanel == null)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems!)
                {
                    var tileItem = new TileItem() { Content = item };
                    tileItem.Click += TileItem_Click;
                    _itemsHostPanel.Children.Add(tileItem);
                }

                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems!)
                {
                    if (ActivatedItem != null && ActivatedItem == item)
                    {
                        ActivatedItem = null;
                    }
                    else
                    {
                        foreach (var activatedItem in _itemsHostPanel.Children)
                        {
                            if (activatedItem == item)
                            {
                                _itemsHostPanel.Children.Remove(activatedItem);
                                break;
                            }
                        }
                    }
                }

                break;

            case NotifyCollectionChangedAction.Reset:
                _itemsHostPanel.Children.Clear();
                ActivatedItem = null;
                break;
        }
    }

    private void TileItem_Click(object? sender, EventArgs e)
    {
        if (_itemsHostPanel == null)
        {
            return;
        }

        if (sender is TileItem tileItem)
        {
            if (Equals(ActivatedItem, tileItem))
            {
                ActivatedItem = null;
                AddToItemsHostPanel(tileItem);
            }
            else
            {
                var oldValue = ActivatedItem;
                ActivatedItem = null;
                if (oldValue != null)
                {
                    AddToItemsHostPanel(oldValue);
                }
                _itemsHostPanel.Children.Remove(tileItem);
                ActivatedItem = tileItem;
            }

            if (_previewItemHost != null)
            {
                _previewItemHost.IsVisible = ActivatedItem != null;
            }

            Grid.SetColumn(_itemsHost, ActivatedItem == null ? 0 : 1);
            Grid.SetColumnSpan(_itemsHost, ActivatedItem == null ? 2 : 1);
        }

        return;

        void AddToItemsHostPanel(TileItem item)
        {
            var index = Items.IndexOf(item.Content!);
            _itemsHostPanel.Children.Insert(index, item);
        }
    }

    protected override Type StyleKeyOverride { get; } = typeof(TileView);

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public double ItemWidth
    {
        get => GetValue(ItemWidthProperty);
        set => SetValue(ItemWidthProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    [Content] public ObservableCollection<object> Items { get; } = new();

    public TileItem? ActivatedItem
    {
        get => GetValue(ActivatedItemProperty);
        set => SetValue(ActivatedItemProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        switch (change.Property.Name)
        {
            case nameof(Rows):
            case nameof(Columns):
                UpdateItemSize();
                break;
        }
    }

    private void UpdateItemSize()
    {
        var rows = Rows == 0 ? 1 : Rows;
        var columns = Columns == 0 ? 1 : Columns;
        ItemWidth = Math.Max(0, Bounds.Width / columns);
        ItemHeight = Math.Max(0, Bounds.Height / rows);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateItemSize();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsHost = e.NameScope.Find<ScrollViewer>("PART_ItemsHost");
        _itemsHostPanel = e.NameScope.Find<WrapPanel>("PART_ItemsPanel");
        _previewItemHost = e.NameScope.Find<ContentControl>("PART_PreviewItemHost");
        if (_itemsHostPanel != null)
        {
            _itemsHostPanel.Children.Clear();
            if (Items.Any())
            {
                foreach (var item in Items)
                {
                    var tileItem = new TileItem() { Content = item };
                    tileItem.Click += TileItem_Click;
                    _itemsHostPanel?.Children.Add(tileItem);
                }
            }
        }
    }
}